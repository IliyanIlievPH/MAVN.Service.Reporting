﻿using System;
using System.Threading.Tasks;
using MAVN.Service.CrossChainTransfers.Contract;
using MAVN.Service.CustomerProfile.Client;
using MAVN.Service.Reporting.Domain;
using MAVN.Service.Reporting.Domain.Models;
using MAVN.Service.Reporting.Domain.Repositories;
using MAVN.Service.Reporting.Domain.Services;

namespace MAVN.Service.Reporting.DomainServices.EventHandlers
{
    public class TransferToInternalCompletedHandler : IEventHandler<TransferToInternalCompletedEvent>
    {
        private const string TxType = "Transfer To Internal Wallet";

        private readonly ICustomerProfileClient _customerProfileClient;
        private readonly ITransactionReportRepository _reportHelper;

        public TransferToInternalCompletedHandler(
            ICustomerProfileClient customerProfileClient,
            ITransactionReportRepository reportHelper
        )
        {
            _customerProfileClient = customerProfileClient;
            _reportHelper = reportHelper;
        }

        public async Task HandleAsync(TransferToInternalCompletedEvent msg)
        {
            // TODO: implement deduplication later

            var receiverResponse = await _customerProfileClient.CustomerProfiles.GetByCustomerIdAsync(msg.CustomerId);
            var receiver = receiverResponse?.Profile;

            await _reportHelper.AddAsync(
                new TransactionReport
                {
                    Id = msg.OperationId,
                    Timestamp = DateTime.UtcNow,
                    Amount = msg.Amount,
                    TransactionType = TxType,
                    Vertical = Vertical.LoyaltySystem.ToString(),
                    Status = TxStatus.Completed.ToString(),
                    ReceiverName = $"{receiver?.FirstName} {receiver?.LastName}",
                    ReceiverEmail = receiver?.Email,
                }
            );
        }
    }
}
