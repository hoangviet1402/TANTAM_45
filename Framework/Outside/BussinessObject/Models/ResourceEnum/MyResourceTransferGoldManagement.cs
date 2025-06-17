using BussinessObject.Models.ResourceEnum.Model;

namespace BussinessObject.Models.ResourceEnum
{
    public partial class MyResourceManagement
    {
        private const ResourceTypeEnum ResourceTransferGoldName = ResourceTypeEnum.TransferGold;

        public static MyResourceDetailModel TransferGoldBaoTri =>
            new MyResourceDetailModel(ResourceTransferGoldName, "BaoTri", "BaoTri");

        public static MyResourceDetailModel CurrentCoinNotEnought => new MyResourceDetailModel(ResourceTransferGoldName,
            "CurrentCoinNotEnought", "CurrentCoinNotEnought");

        public static MyResourceDetailModel TransferGoldFailed =>
            new MyResourceDetailModel(ResourceTransferGoldName, "Failed", "Failed");

        public static MyResourceDetailModel IsCorrectCaptcha =>
            new MyResourceDetailModel(ResourceTransferGoldName, "IsCorrectCaptcha", "IsCorrectCaptcha");

        public static MyResourceDetailModel MaxTransfer =>
            new MyResourceDetailModel(ResourceTransferGoldName, "MaxTransfer", "MaxTransfer");

        public static MyResourceDetailModel MinimumGoldTransfer => new MyResourceDetailModel(ResourceTransferGoldName,
            "MinimumGoldTransfer", "MinimumGoldTransfer");

        public static MyResourceDetailModel TransferGoldNotExitCaptcha =>
            new MyResourceDetailModel(ResourceTransferGoldName, "NotExitCaptcha", "NotExitCaptcha");

        public static MyResourceDetailModel TransferGoldSuccess =>
            new MyResourceDetailModel(ResourceTransferGoldName, "Success", "Success");

        public static MyResourceDetailModel TransferGoldSystemError =>
            new MyResourceDetailModel(ResourceTransferGoldName, "SystemError", "SystemError");

        public static MyResourceDetailModel TransferSuccess =>
            new MyResourceDetailModel(ResourceTransferGoldName, "TransferSuccess", "TransferSuccess");

        public static MyResourceDetailModel TransferCoinComingSoon =>
            new MyResourceDetailModel(ResourceTransferGoldName, "TransferCoinComingSoon", "TransferCoinComingSoon");

        public static MyResourceDetailModel DoiThuongChuyenChipInvalidLevelUser => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipInvalidLevelUser", "DoiThuongChuyenChipInvalidLevelUser");

        public static MyResourceDetailModel DoiThuongChuyenChipInvalidVipUser => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipInvalidVipUser", "DoiThuongChuyenChipInvalidVipUser");

        public static MyResourceDetailModel DoiThuongChuyenChipMaintainance => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipMaintainance", "DoiThuongChuyenChipMaintainance");

        public static MyResourceDetailModel DoiThuongChuyenChipMinVipRequired => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipMinVipRequired", "DoiThuongChuyenChipMinVipRequired");

        public static MyResourceDetailModel DoiThuongChuyenChipRequired =>
            new MyResourceDetailModel(ResourceTransferGoldName, "DoiThuongChuyenChipRequired",
                "DoiThuongChuyenChipRequired");

        public static MyResourceDetailModel DoiThuongChuyenChipUserIsLocked => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipUserIsLocked", "DoiThuongChuyenChipUserIsLocked");

        public static MyResourceDetailModel DoiThuongChuyenChipUserIsLockedTransferChip => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipUserIsLockedTransferChip",
            "DoiThuongChuyenChipUserIsLockedTransferChip");

        public static MyResourceDetailModel DoiThuongChuyenChipUserIsLockedTransferExchange =>
            new MyResourceDetailModel(ResourceTransferGoldName, "DoiThuongChuyenChipUserIsLockedTransferExchange",
                "DoiThuongChuyenChipUserIsLockedTransferExchange");

        public static MyResourceDetailModel DoiThuongChuyenChipUserIsOverloadTransferTime => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipUserIsOverloadTransferTime",
            "DoiThuongChuyenChipUserIsOverloadTransferTime");

        public static MyResourceDetailModel DoiThuongChuyenChipInputReceivedUser => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipInputReceivedUser", "DoiThuongChuyenChipInputReceivedUser");

        public static MyResourceDetailModel DoiThuongChuyenChipTransferYourSelf => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipTransferYourSelf", "DoiThuongChuyenChipTransferYourSelf");

        public static MyResourceDetailModel DoiThuongChuyenChipReceivedAccountNotFound => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipReceivedAccountNotFound",
            "DoiThuongChuyenChipReceivedAccountNotFound");

        public static MyResourceDetailModel DoiThuongChuyenChipReceivedAccountInvalidVip => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipReceivedAccountInvalidVip",
            "DoiThuongChuyenChipReceivedAccountInvalidVip");

        public static MyResourceDetailModel DoiThuongChuyenChipReceivedAccountIsNotAgency => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipReceivedAccountIsNotAgency",
            "DoiThuongChuyenChipReceivedAccountIsNotAgency");

        public static MyResourceDetailModel DoiThuongChuyenChipReceivedAccountIsUser => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipReceivedAccountIsUser",
            "DoiThuongChuyenChipReceivedAccountIsUser");

        public static MyResourceDetailModel DoiThuongChuyenChipReceivedAccountIsLocked => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipReceivedAccountIsLocked",
            "DoiThuongChuyenChipReceivedAccountIsLocked");

        public static MyResourceDetailModel DoiThuongChuyenChipReceivedAccountIsLockedExchange =>
            new MyResourceDetailModel(ResourceTransferGoldName, "DoiThuongChuyenChipReceivedAccountIsLockedExchange",
                "DoiThuongChuyenChipReceivedAccountIsLockedExchange");

        public static MyResourceDetailModel DoiThuongChuyenChipReceivedAccountMinVipRequired =>
            new MyResourceDetailModel(ResourceTransferGoldName, "DoiThuongChuyenChipReceivedAccountMinVipRequired",
                "DoiThuongChuyenChipReceivedAccountMinVipRequired");

        public static MyResourceDetailModel DoiThuongChuyenChipRangeTransferCoin => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipRangeTransferCoin", "DoiThuongChuyenChipRangeTransferCoin");

        public static MyResourceDetailModel DoiThuongChuyenChipMaximumTransferCoin => new MyResourceDetailModel(
            ResourceTransferGoldName, "DoiThuongChuyenChipMaximumTransferCoin",
            "DoiThuongChuyenChipMaximumTransferCoin");

        public static MyResourceDetailModel DoiThuongChuyenChipIsCorrectCaptcha =>
            new MyResourceDetailModel(ResourceTransferGoldName, "IsCorrectCaptcha", "IsCorrectCaptcha");
    }
}