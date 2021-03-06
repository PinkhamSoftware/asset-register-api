﻿using WebApi.Interface;

namespace WebApi.Extensions.Requests
{
    public class SearchAssetApiRequest : IApiRequest
    {
        public int? AssetRegisterVersionId { get; set; }
        public int? SchemeId { get; set; }
        public string Address { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Region { get; set; }
        public string Developer { get; set; }

        public bool IsValid()
        {
            if (AreAllFiltersNullEmptyOrWhiteSpace())
            {
                return false;
            }

            if (!ValidPage())
            {
                return false;
            }

            if (!ValidPageSize())
            {
                return false;
            }

            return true;
        }

        private bool ValidPageSize()
        {
            return PageSize == null || PageSize > 0;
        }

        private bool ValidPage()
        {
            return Page == null || Page > 0;
        }

        private bool SchemeIdInvalidIndex()
        {
            return SchemeId == null || SchemeId <= 0;
        }

        private bool AssetRegisterVersionIdIsNull()
        {
            return AssetRegisterVersionId == null;
        }

        private bool AssetRegisterVersionIdInvalidIndex()
        {
            return AssetRegisterVersionId != null && AssetRegisterVersionId <= 0;
        }

        private bool AreAllFiltersNullEmptyOrWhiteSpace()
        {
            var isInvalidSchemeId =  SchemeIdInvalidIndex();
            var isInvalidAddress = IsEmptyAddress();
            var isInvalidRegion = IsRegionEmpty();
            var isInvalidDeveloper = IsDeveloperEmpty();
            return isInvalidSchemeId && isInvalidAddress && isInvalidRegion && isInvalidDeveloper;
        }

        private bool IsEmptyAddress()
        {
            var isAddressNullEmptyOrWhiteSpace = string.IsNullOrEmpty(Address) || string.IsNullOrWhiteSpace(Address);
            return isAddressNullEmptyOrWhiteSpace;
        }

        private bool IsRegionEmpty()
        {
            var isNullOrEmptyOrWhiteSpace = string.IsNullOrEmpty(Region) || string.IsNullOrWhiteSpace(Region);
            return isNullOrEmptyOrWhiteSpace;
        }

        private bool IsDeveloperEmpty()
        {
            var isNullOrEmptyOrWhiteSpace = string.IsNullOrEmpty(Developer) || string.IsNullOrWhiteSpace(Developer);
            return isNullOrEmptyOrWhiteSpace;
        }
    }
}
