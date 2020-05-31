﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using MAVN.Service.Campaign.Client;
using MAVN.Service.Campaign.Client.Models.Enums;
using MAVN.Service.CustomerProfile.Client;
using MAVN.Service.Reporting.Domain;
using MAVN.Service.Reporting.Domain.Models;
using MAVN.Service.Reporting.Domain.Repositories;
using MAVN.Service.Reporting.Domain.Services;
using MAVN.Service.Staking.Contract.Events;

namespace MAVN.Service.Reporting.DomainServices.EventHandlers
{
    public class ReferralStakeReservedHandler : IEventHandler<ReferralStakeReservedEvent>
    {
        private const string TxType = "Stake Reserved";

        private readonly ICampaignClient _campaignClient;
        private readonly ICustomerProfileClient _customerProfileClient;
        private readonly IReferralInfoFetcher _referralInfoFetcher;
        private readonly ITransactionReportRepository _reportHelper;
        private readonly IVerticalResolver _verticalResolver;
        private readonly ILog _log;

        public ReferralStakeReservedHandler(
            ICampaignClient campaignClient,
            ICustomerProfileClient customerProfileClient,
            IReferralInfoFetcher referralInfoFetcher,
            ITransactionReportRepository reportHelper,
            IVerticalResolver verticalResolver,
            ILogFactory logFactory
        )
        {
            _campaignClient = campaignClient;
            _customerProfileClient = customerProfileClient;
            _referralInfoFetcher = referralInfoFetcher;
            _reportHelper = reportHelper;
            _verticalResolver = verticalResolver;
            _log = logFactory.CreateLog(this);
        }

        public async Task HandleAsync(ReferralStakeReservedEvent msg)
        {
            // TODO: implement deduplication later

            var campaign = await _campaignClient.History.GetEarnRuleByIdAsync(Guid.Parse(msg.CampaignId));
            if (campaign.ErrorCode != CampaignServiceErrorCodes.EntityNotFound)
            {
                _log.Warning("Campaign not found for bonus event",
                    context: new { customerId = msg.CustomerId, error = campaign.ErrorCode.ToString() });
            }

            var senderResponse = await _customerProfileClient.CustomerProfiles.GetByCustomerIdAsync(msg.CustomerId);
            var sender = senderResponse?.Profile;

            var condition = campaign.Conditions?.FirstOrDefault(c => c.Id == msg.CampaignId);
            var vertical = await _verticalResolver.ResolveVerticalAsync(condition?.Type);

            string info = null;
            if (!string.IsNullOrWhiteSpace(msg.ReferralId))
            {
                var referralInfo = await _referralInfoFetcher.FetchReferralInfoAsync(msg.ReferralId);
                if (referralInfo != null)
                    info = $"Staking amount reserved for referring {referralInfo.Email}";
            }

            await _reportHelper.AddAsync(
                new TransactionReport
                {
                    Id = msg.ReferralId,
                    Timestamp =  msg.Timestamp,
                    Amount = msg.Amount,
                    Vertical = vertical.ToString(),
                    Status = TxStatus.Reserved.ToString(),
                    TransactionType = TxType,
                    CampaignName = campaign.Name,
                    Info = info,
                    SenderName = $"{sender?.FirstName} {sender?.LastName}",
                    SenderEmail = sender?.Email,
                    CampaignId = Guid.Parse(msg.CampaignId),
                }
            );
        }
    }
}
