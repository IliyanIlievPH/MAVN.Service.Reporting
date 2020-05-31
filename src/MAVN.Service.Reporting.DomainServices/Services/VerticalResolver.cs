﻿using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using MAVN.Service.Campaign.Client;
using MAVN.Service.Campaign.Client.Models.BonusType;
using MAVN.Service.Reporting.Domain;
using MAVN.Service.Reporting.Domain.Services;

namespace MAVN.Service.Reporting.DomainServices.Services
{
    public class VerticalResolver : IVerticalResolver
    {
        private readonly ICampaignClient _campaignClient;
        private readonly ConcurrentDictionary<string, BonusTypeModel> _bonusTypeDict = new ConcurrentDictionary<string, BonusTypeModel>();

        public VerticalResolver(
            ICampaignClient campaignClient)
        {
            _campaignClient = campaignClient;
        }

        public async Task<Vertical> ResolveVerticalAsync(string conditionType)
        {
            if (string.IsNullOrWhiteSpace(conditionType))
                return Vertical.LoyaltySystem;

            if (!_bonusTypeDict.TryGetValue(conditionType, out var bonusType))
            {
                bonusType = await _campaignClient.BonusTypes.GetByTypeAsync(conditionType);
                _bonusTypeDict.TryAdd(conditionType, bonusType);
            }

            if (!bonusType.Vertical.HasValue)
                return Vertical.Unknown;

            switch (bonusType.Vertical.Value)
            {
                case MAVN.Service.PartnerManagement.Client.Models.Vertical.Hospitality:
                    return Vertical.Hospitality;
                case MAVN.Service.PartnerManagement.Client.Models.Vertical.RealEstate:
                    return Vertical.RealEstate;
                case MAVN.Service.PartnerManagement.Client.Models.Vertical.Retail:
                    return Vertical.Retail;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
