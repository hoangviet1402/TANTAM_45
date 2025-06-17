using BussinessObject.Models.ResourceEnum.Model;

namespace BussinessObject.Models.ResourceEnum
{
    public partial class MyResourceManagement
    {
        private const ResourceTypeEnum ResourcePaymentName = ResourceTypeEnum.Payment;

        public static MyResourceDetailModel UserIsLock =>
            new MyResourceDetailModel(ResourcePaymentName, "UserIsLock", "UserIsLock");

        public static MyResourceDetailModel TransactionNotExists =>
            new MyResourceDetailModel(ResourcePaymentName, "TransactionNotExists", "TransactionNotExists");

        public static MyResourceDetailModel TransactionNotExist2 =>
            new MyResourceDetailModel(ResourcePaymentName, "TransactionNotExist2", "TransactionNotExist2");

        public static MyResourceDetailModel TransactionFail =>
            new MyResourceDetailModel(ResourcePaymentName, "TransactionFail", "TransactionFail");

        public static MyResourceDetailModel TransactionExcuteFail =>
            new MyResourceDetailModel(ResourcePaymentName, "TransactionExcuteFail", "TransactionExcuteFail");

        public static MyResourceDetailModel SuccessButAddGoldFail =>
            new MyResourceDetailModel(ResourcePaymentName, "SuccessButAddGoldFail", "SuccessButAddGoldFail");

        public static MyResourceDetailModel PaymentSuccess =>
            new MyResourceDetailModel(ResourcePaymentName, "Success", "Success");

        public static MyResourceDetailModel SmsPaymentIsEnable =>
            new MyResourceDetailModel(ResourcePaymentName, "SmsPaymentIsEnable", "SmsPaymentIsEnable");

        public static MyResourceDetailModel SmsPaymentIsCorrect =>
            new MyResourceDetailModel(ResourcePaymentName, "SmsPaymentIsCorrect", "SmsPaymentIsCorrect");

        public static MyResourceDetailModel SmsPaymentError1 =>
            new MyResourceDetailModel(ResourcePaymentName, "SmsPaymentError1", "SmsPaymentError1");

        public static MyResourceDetailModel SmsAritcleNotExists =>
            new MyResourceDetailModel(ResourcePaymentName, "SmsAritcleNotExists", "SmsAritcleNotExists");

        public static MyResourceDetailModel Pending =>
            new MyResourceDetailModel(ResourcePaymentName, "Pending", "Pending");

        public static MyResourceDetailModel NumberDepositOnDay =>
            new MyResourceDetailModel(ResourcePaymentName, "NumberDepositOnDay", "NumberDepositOnDay");

        public static MyResourceDetailModel NotExitCaptcha =>
            new MyResourceDetailModel(ResourcePaymentName, "NotExitCaptcha", "NotExitCaptcha");

        public static MyResourceDetailModel NoData =>
            new MyResourceDetailModel(ResourcePaymentName, "NoData", "NoData");

        public static MyResourceDetailModel MsgDoiThe3 =>
            new MyResourceDetailModel(ResourcePaymentName, "MsgDoiThe3", "MsgDoiThe3");

        public static MyResourceDetailModel MsgDoiThe2 =>
            new MyResourceDetailModel(ResourcePaymentName, "MsgDoiThe2", "MsgDoiThe2");

        public static MyResourceDetailModel MsgDoiThe1 =>
            new MyResourceDetailModel(ResourcePaymentName, "MsgDoiThe1", "MsgDoiThe1");

        public static MyResourceDetailModel KhongTheDoiThuong =>
            new MyResourceDetailModel(ResourcePaymentName, "KhongTheDoiThuong", "KhongTheDoiThuong");

        public static MyResourceDetailModel InputMoneySuccess =>
            new MyResourceDetailModel(ResourcePaymentName, "InputMoneySuccess", "InputMoneySuccess");

        public static MyResourceDetailModel InCorrectCardCode =>
            new MyResourceDetailModel(ResourcePaymentName, "InCorrectCardCode", "InCorrectCardCode");

        public static MyResourceDetailModel InCorrectCard =>
            new MyResourceDetailModel(ResourcePaymentName, "InCorrectCard", "InCorrectCard");

        public static MyResourceDetailModel InCorrectCaptcha =>
            new MyResourceDetailModel(ResourcePaymentName, "InCorrectCaptcha", "InCorrectCaptcha");

        public static MyResourceDetailModel HaveData =>
            new MyResourceDetailModel(ResourcePaymentName, "HaveData", "HaveData");

        public static MyResourceDetailModel FunctionNotExists =>
            new MyResourceDetailModel(ResourcePaymentName, "FunctionNotExists", "FunctionNotExists");

        public static MyResourceDetailModel PaymentFailed =>
            new MyResourceDetailModel(ResourcePaymentName, "Failed", "Failed");

        public static MyResourceDetailModel ErrorSystem =>
            new MyResourceDetailModel(ResourcePaymentName, "ErrorSystem", "ErrorSystem");

        public static MyResourceDetailModel ErrorSeri =>
            new MyResourceDetailModel(ResourcePaymentName, "ErrorSeri", "ErrorSeri");

        public static MyResourceDetailModel ErrorPin =>
            new MyResourceDetailModel(ResourcePaymentName, "ErrorPin", "ErrorPin");

        public static MyResourceDetailModel ChoiseCardType =>
            new MyResourceDetailModel(ResourcePaymentName, "ChoiseCardType", "ChoiseCardType");

        public static MyResourceDetailModel CardType =>
            new MyResourceDetailModel(ResourcePaymentName, "CardType", "CardType");

        public static MyResourceDetailModel CardSeri =>
            new MyResourceDetailModel(ResourcePaymentName, "CardSeri", "CardSeri");

        public static MyResourceDetailModel CardNumber =>
            new MyResourceDetailModel(ResourcePaymentName, "CardNumber", "CardNumber");

        public static MyResourceDetailModel CardAmount =>
            new MyResourceDetailModel(ResourcePaymentName, "CardAmount", "CardAmount");

        public static MyResourceDetailModel BaoTriCard =>
            new MyResourceDetailModel(ResourcePaymentName, "BaoTriCard", "BaoTriCard");

        public static MyResourceDetailModel AccountNotExist => new MyResourceDetailModel(ResourcePaymentName,
            "OpenAuthConfirmLogin_NotExistAccount", "OpenAuthConfirmLogin_NotExistAccount");

        public static MyResourceDetailModel BuySuccess =>
            new MyResourceDetailModel(ResourcePaymentName, "BuySuccess", "BuySuccess");

        public static MyResourceDetailModel NotEnoughtMoneyToBuy =>
            new MyResourceDetailModel(ResourcePaymentName, "NotEnoughtMoneyToBuy", "NotEnoughtMoneyToBuy");

        public static MyResourceDetailModel DepositNotificationSuccess => new MyResourceDetailModel(ResourcePaymentName,
            "DepositNotificationSuccess", "DepositNotificationSuccess");

        public static MyResourceDetailModel DepositSmsNotificationSuccess =>
            new MyResourceDetailModel(ResourcePaymentName, "DepositSmsNotificationSuccess",
                "DepositSmsNotificationSuccess");

        public static MyResourceDetailModel DepositNotificationFail => new MyResourceDetailModel(ResourcePaymentName,
            "DepositNotificationFail", "DepositNotificationFail");

        public static MyResourceDetailModel DepositTitleMessage =>
            new MyResourceDetailModel(ResourcePaymentName, "DepositTitleMessage", "DepositTitleMessage");

        public static MyResourceDetailModel DepositCaptchaUncorrect => new MyResourceDetailModel(ResourcePaymentName,
            "DepositCaptchaUncorrect", "DepositCaptchaUncorrect");

        public static MyResourceDetailModel DepositPinCardUncorrect => new MyResourceDetailModel(ResourcePaymentName,
            "DepositPinCardUncorrect", "DepositPinCardUncorrect");

        public static MyResourceDetailModel DepositEmptyPin =>
            new MyResourceDetailModel(ResourcePaymentName, "DepositEmptyPin", "DepositEmptyPin");

        public static MyResourceDetailModel DepositRequiredLogin =>
            new MyResourceDetailModel(ResourcePaymentName, "DepositRequiredLogin", "DepositRequiredLogin");

        public static MyResourceDetailModel DepositCardInfoUncorrect => new MyResourceDetailModel(ResourcePaymentName,
            "DepositCardInfoUncorrect", "DepositCardInfoUncorrect");

        public static MyResourceDetailModel DepositSupportText =>
            new MyResourceDetailModel(ResourcePaymentName, "DepositSupportText", "DepositSupportText");

        public static MyResourceDetailModel DepositProccessError =>
            new MyResourceDetailModel(ResourcePaymentName, "DepositProccessError", "DepositProccessError");

        public static MyResourceDetailModel DepositCardInfoUncorrectTryAgain =>
            new MyResourceDetailModel(ResourcePaymentName, "DepositCardInfoUncorrectTryAgain",
                "DepositCardInfoUncorrectTryAgain");

        public static MyResourceDetailModel DepositCardAmountNotAllow => new MyResourceDetailModel(ResourcePaymentName,
            "DepositCardAmountNotAllow", "DepositCardAmountNotAllow");

        public static MyResourceDetailModel DepositDelayTime =>
            new MyResourceDetailModel(ResourcePaymentName, "DepositDelayTime", "DepositDelayTime");

        public static MyResourceDetailModel DepositMultipleFailed =>
            new MyResourceDetailModel(ResourcePaymentName, "DepositMultipleFailed", "DepositMultipleFailed");

        public static MyResourceDetailModel DepositCardIsUsed =>
            new MyResourceDetailModel(ResourcePaymentName, "DepositCardIsUsed", "DepositCardIsUsed");

        public static MyResourceDetailModel DepositCreateTransactionFail =>
            new MyResourceDetailModel(ResourcePaymentName, "DepositCreateTransactionFail",
                "DepositCreateTransactionFail");

        public static MyResourceDetailModel DepositTransactionTimeOut => new MyResourceDetailModel(ResourcePaymentName,
            "DepositTransactionTimeOut", "DepositTransactionTimeOut");

        public static MyResourceDetailModel DepositTransactionError => new MyResourceDetailModel(ResourcePaymentName,
            "DepositTransactionError", "DepositTransactionError");

        public static MyResourceDetailModel DepositSelectCardAmountWrong =>
            new MyResourceDetailModel(ResourcePaymentName, "DepositSelectCardAmountWrong",
                "DepositSelectCardAmountWrong");

        public static MyResourceDetailModel DepositTransactionChargeWithCallback => new MyResourceDetailModel(
            ResourcePaymentName, "DepositTransactionChargeWithCallback", "DepositTransactionChargeWithCallback");

        public static MyResourceDetailModel DoiThuongContentModalSuccess =>
            new MyResourceDetailModel(ResourcePaymentName, "DoiThuongContentModalSuccess",
                "DoiThuongContentModalSuccess");

        public static MyResourceDetailModel DoiThuongContentModalSuccessTrue =>
            new MyResourceDetailModel(ResourcePaymentName, "DoiThuongContentModalSuccessTrue",
                "DoiThuongContentModalSuccessTrue");

        public static MyResourceDetailModel DoiThuongContentModalSuccessAis =>
            new MyResourceDetailModel(ResourcePaymentName, "DoiThuongContentModalSuccessAis",
                "DoiThuongContentModalSuccessAis");

        public static MyResourceDetailModel DoiThuongContentModalSuccessDtac =>
            new MyResourceDetailModel(ResourcePaymentName, "DoiThuongContentModalSuccessDtac",
                "DoiThuongContentModalSuccessDtac");

        public static MyResourceDetailModel DoiThuongTitleOfflineMessageSuccess => new MyResourceDetailModel(
            ResourcePaymentName, "DoiThuongTitleOfflineMessageSuccess", "DoiThuongTitleOfflineMessageSuccess");

        public static MyResourceDetailModel DoiThuongContentOfflineMessageSuccess => new MyResourceDetailModel(
            ResourcePaymentName, "DoiThuongContentOfflineMessageSuccess", "DoiThuongContentOfflineMessageSuccess");

        public static MyResourceDetailModel DoiThuongContentOfflineMessageSuccessTrue => new MyResourceDetailModel(
            ResourcePaymentName, "DoiThuongContentOfflineMessageSuccessTrue",
            "DoiThuongContentOfflineMessageSuccessTrue");

        public static MyResourceDetailModel DoiThuongContentOfflineMessageSuccessAis => new MyResourceDetailModel(
            ResourcePaymentName, "DoiThuongContentOfflineMessageSuccessAis",
            "DoiThuongContentOfflineMessageSuccessAis");

        public static MyResourceDetailModel DoiThuongContentOfflineMessageSuccessDtac => new MyResourceDetailModel(
            ResourcePaymentName, "DoiThuongContentOfflineMessageSuccessDtac",
            "DoiThuongContentOfflineMessageSuccessDtac");
    }
}