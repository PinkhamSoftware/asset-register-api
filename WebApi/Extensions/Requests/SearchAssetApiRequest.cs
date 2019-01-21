using WebApi.Interface;

namespace WebApi.Extensions.Requests
{
    public class SearchAssetApiRequest : IApiRequest
    {
        public int? AssetRegisterVersionId { get; set; }
        public int? SchemeId { get; set; }
        public string Address { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public bool IsValid()
        {
            if (InvalidSchemeID())
            {
                return false;
            }

            if (SchemeIsNullAndAddressIsEmpty())
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

            if (AssetRegisterVersionIdIsNull())
            {
                return false;
            }

            if (AssetRegisterVersionIdInvalidIndex())
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

        private bool InvalidSchemeID()
        {
            return SchemeIsNullAndAddressIsEmpty() || SchemeIdInvalidIndex();
        }

        private bool SchemeIdInvalidIndex()
        {
            return SchemeId != null && SchemeId <= 0;
        }

        private bool AssetRegisterVersionIdIsNull()
        {
            return AssetRegisterVersionId == null;
        }


        private bool AssetRegisterVersionIdInvalidIndex()
        {
            return AssetRegisterVersionId != null && AssetRegisterVersionId <= 0;
        }

        private bool SchemeIsNullAndAddressIsEmpty()
        {
            return SchemeId == null && IsEmptyAddress();
        }

        private bool IsEmptyAddress()
        {
            return (string.IsNullOrEmpty(Address) || string.IsNullOrWhiteSpace(Address));
        }
    }
}
