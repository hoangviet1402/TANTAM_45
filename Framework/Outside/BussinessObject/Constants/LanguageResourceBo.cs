using ResxLanguagesUtility;

namespace BussinessObject.Constants
{
    public static class LanguageResourceBo
    {
        public static string Language => ResxLanguages.GetCurrentLanguage;

        #region

        // BO
        public const string CreateTranlogError = "Lỗi khởi tạo giao dịch";
        public const string AddGoldUserFromEvent = "Tặng Gold cho user mới";
        public const string AddGoldUserFromEventError = "Lỗi tặng Gold cho user";
        public const string AddGoldUserFirstChargeCardInDay = "Tặng Gold cho user nạp thẻ đầu tiên trong ngày";
        public const string AddGoldUserChargeCard = "Tặng Gold cho user nạp thẻ";
        public const string AddGoldUserChargeIAP = "Tặng Gold cho user nạp IAP";
        public const string NotPromoCard = "Thẻ không được khuyến mãi";

        public const string EvnetNapTheTichCodeSuccess =
            "Bạn được tặng {0} code tham gia xổ số. Vui lòng vào hộp thư cá nhân xem mã code!";

        public const string EvnetNapTheTichCodeSystemError =
            "Lỗi cấp mã code. Vui lòng liên hệ ban quản trị để được cấp lại!";

        public const string AddGoldUserRegBcst = "Tặng Gold cho user đăng ký game Bắn cá săn thưởng";
        public const string AddGoldUserHall = "Tặng Gold cho user hết xu khi ra sảnh";
        public const string BonusGoldAvatar = "Tặng Gold cho user khi thay đổi avatar";
        public const string BonusShareSuccess = "Nhận thưởng thành công";
        public const string BonusShareError = "Nhận thưởng không thành công";
        public const string RegisterRefererPubUser = "Mời bạn tham gia game";
        public const string RegisterRefererPubUserAccepted = "Đã chấp nhận lời mời";
        public const string DoImportCardResponseError = "Lỗi không đọc được thông tin thẻ trả về";
        public const string DoImportCardImportError = "Lỗi nhập thẻ vào kho";
        public const string DoImportCardSupplierExportError = "Lỗi xuất thẻ từ nhà cung cấp";
        public const string DoExportCardNotSelectCard = "Vui lòng chọn loại thẻ cần đổi";

        public const string DoExportCardIsMaintain =
            "Chức năng hiện tại đang được bảo trì nâng cấp. Bạn vui lòng quay lại sau!!!";

        public const string DoExportCardOutOfLimit =
            "Bạn đã hết hạn mức đổi thẻ trong ngày. Vui lòng quay lại vào ngày hôm sau!";

        public const string DoExportCardOutOfRangeTime = "Mời bạn đổi thẻ tiếp theo sau thẻ trước {0} phút. Xin cảm ơn";
        public const string DoExportCardNotEnoughGold = "Số dư tài khoản không đủ để đổi thẻ cào";
        public const string DoExportCardNotEnoughGoldOffset = "Bạn không đủ số dư tối thiểu {0:#,##0} gold sau khi đổi";
        public const string DoExportCardError = "Có lỗi trong quá trình đổi thẻ cào. Bạn vui lòng thử lại sau!";
        public const string DoExportCardErrorAdmin = "Lỗi đổi thẻ cào. Bạn vui lòng liên hệ Admin để được hổ trợ!";

        public const string DoExportCardTimeOut =
            "Lỗi đổi thẻ cào. Bạn vui lòng liên hệ Admin để được hổ trợ! [Code = 3]";

        public const string DoExportCardSupGold = "Trừ gold khi đổi thẻ";
        public const string DoExportCardUpdateAccountError = "Lỗi cập nhật số dư tài khoản";
        public const string DoExportCardOutOflimitCard = "Thẻ bạn muốn đổi đã hết, bạn vui lòng thử lại sau!";

        public const string DoExportCardSuccess =
            "Yêu cầu đổi thẻ {0}({1}VND) của bạn đã thành công. Bạn vui lòng chờ Admin kiểm duyệt. Thẻ sẽ được gửi vào HỘP THƯ CÁ NHÂN sau ít phút nữa!";

        public const string CashOutDataError = "Dữ liệu không hợp lệ";
        public const string CashOutError = "Có lỗi trong quá trình thực hiện. Bạn vui lòng thử lại sau!";

        public const string OutOfGoldCashOut =
            "Bạn đã hết hạn mức rút tiền trong ngày. Vui lòng quay lại vào ngày hôm sau!";

        public const string CashOutNotEnoughGold = "Số dư tài khoản không đủ để thực hiện giao dịch!";
        public const string CashOutAccountVnKashIsNotExit = "Thông tin tài khoản vnKash không hợp lệ!";
        public const string CashOutAccountVtcPayIsNotExit = "Thông tin tài khoản vtcPay không hợp lệ!";
        public const string CashOutAVnKashNotEnoughGold = "Tai khoan daily vnkash khong du tien | Balance: {0}";
        public const string AccountCashOut = "{0} rút tiền về tài khoản";
        public const string CashOutSupGold = "Trừ gold khi rút tiền về vnkash";
        public const string CashOutBaoKimSupGold = "Trừ gold khi rút tiền về BaoKim";

        public const string CashOutTimeOut =
            "Lỗi chuyển tiền sang tài khoản vnkash. Bạn vui lòng liên hệ CSKH để hổ trợ!";

        public const string CashOutTimeOutBaoKim =
            "Lỗi chuyển tiền sang tài khoản BaoKim. Bạn vui lòng liên hệ CSKH để hổ trợ!";

        public const string CashOutReturnGold = "Lỗi timed out cần kiểm tra lại giao dịch trước khi hoàn Xu";

        public const string CashOutReturnGoldCSKH =
            "Lỗi chuyển tiền sang tài khoản vnkash. Bạn vui lòng liên hệ CSKH để hoàn lại Xu!";

        public const string CashOutReturnGoldCskhBaoKim =
            "Lỗi chuyển tiền sang tài khoản BaoKim. Bạn vui lòng liên hệ CSKH để hoàn lại Xu!";

        public const string CashOutErrorReturnGoldCSKH =
            "Có lỗi trong quá trình xử lý. Bạn vui lòng liên hệ CSKH để được hổ trợ!";

        public const string CashOutSuccess = "Bạn đã rút thành công {0} về ví Vnkash";
        public const string CashOutSuccessBaoKim = "Bạn đã rút thành công {0} về BaoKim";
        public const string DepositMinMoney = "Số tiền nạp tối thiểu là 10.000 và phải chia hết cho 1.000!";
        public const string Depositfail = "Giao dịch thất bại vui lòng thử lại sau!";
        public const string DepositDealfail = "Giao dịch không hợp lệ!";
        public const string DepositReview = "Giao dịch đang được kiểm duyệt!";
        public const string DepositAddGoldFail = "Lỗi nạp Xu vào tài khoản. Vui lòng liên hệ CSKH để được hổ trợ!";
        public const string DepositAddGoldSuccess = "Nạp Tiền Thành Công";
        public const string DepositCreatePostDataFail = "Lỗi tạo chuỗi Post Data";
        public const string DepositProcessing = " Thanh toan don hang: ";
        public const string DepositDelay = "Mỗi lần nạp thẻ phải cách nhau {0} phút";
        public const string SubGoldItem = "Trừ xu khi đổi vật phẩm";
        public const string OutOfTurnCard = " - Hết lượt đổi thẻ";
        public const string OutOfTurnStar = " - Hết lượt đổi sao";
        public const string NotEnoughStar = " - Không đủ sao chuyển";
        public const string NotEnoughCardWithProductId = " - Hết thẻ với ProductId: ";

        public const string MessageSendItemSuccess =
            "Cảm ơn bạn đã tham gia sự kiện \"Tích sao đổi quà\"<br/> Mã số thẻ của bạn là: <b> {0} </b><br/> Số seri của bạn là: <b> {1} </b><br/> Mệnh giá thẻ: <b> {2} VNĐ</b><br/> Loại thẻ: <b> {3} </b><br><br> Chúc bạn chơi game vui vẻ !";

        public const string TransferGoldFail = "Không thể chuyển gold. Bạn phải thoát khỏi game {0} và chờ 2 phút.";
        public const string AccountIsNotExit = "Thông tin tài khoản không hợp lệ!";

        public const string AccountIsLock = "Tài khoản đã bị khóa";

        // Enum
        public const string HasBeenUpdated = "Tài khoản đã được cập nhật trước đó!";
        public const string InvalidUpdating = "Dữ liệu cập nhật không đúng!";
        public const string SendingMailError = "Lỗi trong quá trình gửi mail";
        public const string SuccessWithGold = "Tài khoản cập nhật thành công và được tặng Gold";
        public const string AvatarNotExists = "User không tồn tại";
        public const string UserNotExists = "Avatar không tồn tại";
        public const string Failed = "Thất bại";
        public const string OutOfDate = "Hết hạn";
        public const string NotYetSms = "Chưa nhắn tin";
        public const string WrongCode = "Sai mã Code";
        public const string OutOfTimes = "Quá số lần quy định";
        public const string None = "Không có referer";
        public const string GoogleSearch = "Tìm kiếm Google";
        public const string Facebook = "Từ Facebook";
        public const string Mobile = "Từ di động";
        public const string GoogleAdword = "Quảng cáo Google";
        public const string FacebookApp = "Từ Facebook App";
        public const string EmailError = "Tài khoản đã được cập nhật trước đó!";
        public const string EmailExist = "Địa chỉ email đã tồn tại, vui lòng nhập lại!";
        public const string Success = "Thành công";
        public const string PassOrEmailError = "Tên đăng nhập hoặc mật khẩu không chính xác";
        public const string BanNick = "Tài khoản đã bị khóa";
        public const string BanNickForever = "Tài khoản đã bị khóa vĩnh viễn";
        public const string UserIsNotActive = "Tài khoản chưa được kích hoạt";
        public const string UsingAnotherDevice = "User đang dùng máy khác";
        public const string NonexistSecurityCode = "Mã bảo vệ không tồn tại";
        public const string InvalidSecurityCode = "Mã bảo mật không chính xác";
        public const string SuccessWithSecure = "Thành công và có mã bảo vệ";
        public const string NonExistsAccount = "Tài khoản không tồn tại";
        public const string UsingSMSSecure = "Tài khoản dùng bảo mật bằng SMS";
        public const string UsingNormalSecure = "Tài khoản dùng bảo mật";
        public const string OtpExpire = "Mã bảo vệ đã được sử dụng. Vui lòng soạn tin lấy lại mã bảo vệ mới!";
        public const string SystemError = "Lỗi hệ thống";
        public const string InvalidCaptchaCode = "Mã bảo vệ không chính xác";
        public const string Unknown = "Không xác định";
        public const string Web = "Web - Platform";
        public const string Android = "Mobile - HĐH Android";
        public const string Ios = "Mobile - HĐH IOs";
        public const string WindowPhone = "Mobile - HĐH Window phone";
        public const string PlatformIdEnumFacebook = "Web - Facebook Platform";
        public const string NotActivate = "User chưa được kích hoạt";
        public const string UsingSMS = "User đã active chức năng bảo vệ tài khoản bằng SMS";
        public const string Normal = "User sử dụng chức năng bảo vệ tài khoản";
        public const string AllType = "User sử dụng cả 2 chức năng bảo vệ tài khoản";
        public const string InvalidNickName = "Tên đăng nhập không hợp lệ";

        public const string ExistedAccount = "Tài khoản đã tồn tại";
        public const string InvalidDisplayName = "Tên hiển thị đã tồn tại";
        public const string NotUse = "User hiện không có sử dụng chức năng này";
        public const string HasBeenUpdate = "Đã cập nhật trước đó";
        public const string UserNotRegular = "User không hợp lệ";
        public const string NotSelectQuestion = "Chưa chọn câu hỏi";
        public const string NotAnswer = "Chưa trả lời câu hỏi";
        public const string AnswerNotRegular = "Câu trả lời không hợp lệ";
        public const string AnswerHasSignChar = "Câu trả lời không được có dấu";
        public const string IsEnough = "Bang đã đủ thành viên";
        public const string IsMember = "User đã gia nhập bang";
        public const string IsWaiting = "User đã gửi lời mời trước đó và chờ xét duyệt";
        public const string NotEnoughLevel = "User không đủ tiền và level để gia nhập";
        public const string NotPermission = "User không có quyền xử lý";
        public const string Admin = "Admin vinh danh";
        public const string Association = "Bang hội vinh danh";
        public const string Exp = "Tìm theo điểm kinh nghiệm";
        public const string PubUserId = "Tìm theo mã tài khoản";
        public const string NickName = "Tìm theo nickname";
        public const string NameExists = "Tên bang đã tồn tại";
        public const string NameNotExists = "Tên bang chưa có";
        public const string DaThamGiaBangHoi = "Đã tham gia bang hội";
        public const string ChuaCapNhapThongTin = "Chưa cập nhật thông tin";
        public const string KhongDuGold = "không đủ gold tạo bang hội";
        public const string DuplicationName = "Tên bang đã tồn tại";
        public const string IsNotOwner = "Không phải là bang chủ";
        public const string IsNotEnoughGold = "Không đủ gold";
        public const string IsEnoughMember = "Bang đã đủ thành viên";
        public const string IsBelongToAssociation = "User đã là thành viên của bang";
        public const string HasSent = "User đã được gửi lời mời";
        public const string SameUser = "User không thể mời chính mình";
        public const string InvalidMessage = "User chưa nhập tin nhắn";
        public const string UserLeft = "User không còn trong bang";
        public const string AccessDenied = "User thực hiện kick không đủ quyền";
        public const string UserTuRoi = "User không còn trong bang";
        public const string OwnerKick = "User thực hiện kick không đủ quyền";
        public const string SubOwnerKick = "User thực hiện kick không đủ quyền";
        public const string UserHasSent = "User đã gửi lời mời trước đó và chờ được xét duyệt";
        public const string IsNotEnoughCondition = "User không đủ tiền và level để gia nhập";
        public const string IsExistsAnotherAss = "User đang là thành viên của bang khác";
        public const string EnoughSubOwner = "Bang đã đủ bang phó";
        public const string AlreadyDone = "User đã là bang phó, hoặc đã không còn là bang phó, tùy vào loại";
        public const string NotOwner = "User lập bang phó không phải là bang chủ";
        public const string SameOwnUser = "User được lập bang phó đang là bang chủ";
        public const string NotMember = "User được lập bang phó không phải là thành viên trong bang";
        public const string Round1 = "Đợt 1";
        public const string Round2 = "Đợt 2";
        public const string Round3 = "Đợt 3";
        public const string OutOfStars = "Vượt quá số sao";
        public const string CardPartnerFail = "Lỗi đổi thẻ cào từ đối tác";
        public const string Card = "Chuyển sang thẻ cào";
        public const string Item = "Chuyển sang thẻ vật phẩm";
        public const string StarExchangeGold = "Chuyển sang gold";
        public const string Vong1 = "Vòng loại";
        public const string Vong2 = "Bán kết";
        public const string Vong3 = "Chung kết";
        public const string InvalidSender = "Người gửi không tồn tại";
        public const string InvalidReceiver = "Người nhận không tồn tại";
        public const string NotEnoughGold = "Không đủ gold";
        public const string InvalidGold = "Số Gold cần chuyển phải >= 20.000,mời nhập lại!";
        public const string IsNotSameAssociation = "User nhận không cùng bang hội";
        public const string OutOfTime = "Hết số lần chuyển Gold bang hội";
        public const string UserInGame = "User đang chơi game";
        public const string RangerGoldMaxOnDay = "Vượt quá số gold được chuyển trong ngày";
        public const string NotEnoughLevelGame = "Bạn chưa đủ level game để chuyển gold";
        public const string ExistedName = "Tên bang đã tồn tại";
        public const string UserInAnotherGuild = "Bạn đang trong một bang khác";
        public const string NoRight = "User không có quyền xóa";
        public const string NotExistedGuild = "Bang không tồn tại";
        public const string ValueIncorrect = "Lượng Gold không phù hợp";
        public const string CommentNotMember = "User không phải thành viên";
        public const string NotExistsComment = "Comment cha không tồn tại";
        public const string NotExistsGuild = "Bang không tồn tại";
        public const string JoinIsWaiting = "User đã xin vào bang và đang chờ duyệt";
        public const string CannotKickOwner = "Bang chủ không thể kick chính mình";
        public const string UserNotInGuild = "Bạn không ở trong bang";
        public const string CannotLeftOwner = "Bang chủ không được rời bang";
        public const string NotEnough = "Gold đại lý không đủ chuyển";
        public const string NotExists = "ID không tồn tại";
        public const string FailTransaction = "Giao dịch thất bại";
        public const string ExistantTransaction = "Giao dịch đã tồn tại";
        public const string InvalidVipPoint = "Có lỗi khi cộng Vip Point";
        public const string NapGold = "Kiểm tra giao dịch từ nạp gold";
        public const string KiemTraGiaoDich = "Kiểm tra giao dịch do user hoặc admin kích hoạt";
        public const string ThatBai = "Giao dịch thất bại do không cập nhật được";
        public const string DaiLyKhongDuGold = "Đại lý không đủ gold";
        public const string GiaoDichKhongTonTai = "Giao dịch không tồn tại";
        public const string TheDaSuDung = "Thẻ đã sử dụng";
        public const string CardAmountInvalid = "Mệnh giá thẻ không đúng";
        public const string DepositCardError = "Nạp sai";
        public const string CardTypeInvalid = "Loại thẻ không đúng";
        public const string TransactionInvalid = "Mã giao dịch không đúng";
        public const string GiaoDichThatBai = "Giao dịch thất bại từ ngân lượng";
        public const string CongVipPointLoi = "Cộng VipPoint không thành công";
        public const string GiaoDichLoi = "Giao dịch lỗi, được lưu lại để xử lý sau";
        public const string CongGoldPromotionVipError = "Cộng gold, khuyến mãi và vip không thành công";
        public const string CongGoldPromotionError = "Cộng gold, khuyến mãi không thành công";
        public const string CongGoldVipError = "Cộng gold, vip không thành công";
        public const string CongPromotionVipError = "Cộng khuyến mãi và vip không thành công";
        public const string CongGoldError = "Cộng gold không thành công";
        public const string CongPromotionError = "Cộng khuyến mãi không thành công";
        public const string Proccessing = "Đang xử lý";
        public const string TransactionProccessingAndWait = "Giao dịch đang xử lý, vui lòng đợi trong giây lát";
        public const string FailWhenCallUseCard = "Lỗi khi gọi đối tác gạch thẻ";
        public const string SuccessAllNotGold = "Nạp thẻ thành công nhưng không nhận được gold";
        public const string SuccessAllNotPromotion = "Không nhận được khuyến mãi";
        public const string SuccessAllNotVipPoint = "Không nhận được Vip Point";
        public const string SuccessAllNotGoldAndVip = "Không nhận được Gold và Vip Point";
        public const string SuccessAllNotPromotionAndVipPoint = "Không nhận được khuyến mãi và Vip Point";
        public const string SuccessAllNotGoldAndPromotion = "Không nhận được gold và khuyến mãi";
        public const string SuccessNotGoldAndPromotionAndVip = "Không nhận đượcgold, khuyến mãi và Vip Point";
        public const string UserRequestCheckTransactionError = "User yêu cầu kiểm tra trạng thái thẻ lỗi";
        public const string TimeOut = "Nạp thẻ bị timeout";
        public const string CallbackProcessing = "Thẻ đã gửi cho đối tác";
        public const string CallbackError = "Đối tác trả về chi tiết lỗi";
        public const string CallbackReturnDepositAgain = "Đối tác trả về yêu cầu nạp lại";
        public const string FailWhenCallGetStatus = "Lỗi khi gọi đối tác tra cứu trạng thái";
        public const string DelayToNextDeposit = "Chờ tới lượt nạp thẻ tiếp theo";
        public const string MemberToMember = "Thành viên chuyển cho thành viên";
        public const string AssociationToMember = "Bang chuyển cho thành viên";
        public const string KhongDuGoldAddOrSub = "Không đủ gold";
        public const string KhongThucHienLenhNao = "Không thực hiện lệnh nào";
        public const string CongGoldKhongThanhCong = "Cộng gold không thành công";
        public const string LoiException = "Lỗi Exception";
        public const string NapGoldTheCao = "Nạp gold qua thẻ cào";
        public const string NapGoldTheCaoKhuyenMai100 = "Khuyến mãi % qua thẻ cào";
        public const string NapGoldNganLuong = "Nạp gold qua ngân lượng";
        public const string NapGoldNganLuongKhuyenMai = "Khuyến mãi nạp gold qua ngân lượng";
        public const string NapGoldSms = "Khuyến mãi nạp gold qua sms";
        public const string NapGoldSmsKhuyenMai = "Khuyến mãi nạp gold qua sms";
        public const string NapGoldTheCaoKhuyenMaiTiepSuc = "Khuyến mãi tiếp sức qua thẻ cào";
        public const string NapGoldTheCaoKhuyenMaiTihcLuy = "Khuyến mãi tích lũy qua thẻ cào";
        public const string NapGoldTheCaoKhuyenMaiChonLoc = "Khuyến mãi chọn lọc qua thẻ cào";
        public const string NapGoldTheCaoKhuyenMaiNgayVang = "Khuyến mãi ngày vàng qua thẻ cào";
        public const string LiXiTet = "Lì xì tết";
        public const string ChuyenGoldTuTaiKhoanVaoViNganHang = "Chuyển tiền vào tài khoản vào Ví ngân hàng";
        public const string ChuyenGoldTuViNganHangVaoTaiKhoan = "Chuyển tiền từ Ví ngân hàng vào tài khoản";
        public const string CongGoldEvent03 = "Tặng Gold cho User Eway đăng nhập trên Mwork";
        public const string DoiSaoRaTheCao = "Đổi sao ra thẻ cào";
        public const string DoiSaoRaGold = "Đổi sao ra gold";
        public const string DoiSaoRaVatPham = "Đổi sao ra quà";
        public const string NapGoldDaiLy = "Mua Gold đai lý";
        public const string NapGoldDaiLyKhuyenMai = "Khuyến mãi mua gold đại lý";
        public const string NapGoldDaiLyKhuyenMaiMuaGoldChamMoc = "Khuyến mãi mua gold đại lý chạm mốc";
        public const string TriAnVip = "Tri ân Vip";
        public const string NapTienGoogleWallet = "Nạp tiền bằng Ví Google";
        public const string NapTienAppleStore = "Nạp tiền qua Apple Store";
        public const string DoiDoanhThuRaGold = "Đổi doanh thu ra Gold";
        public const string NhanThuongVQXS = "Nhận thưởng Vòng quay";
        public const string NhanThuongRaSanh = "Nhận thưởng Ra sảnh";
        public const string KhuyenMaiUserVIP = "Khuyến mãi USER VIP";
        public const string NhanThuongThanhTuu = "Nhận thưởng thành tựu";
        public const string KhuyenMaiPrivateUserReal = "Khuyến mãi USER REAL";
        public const string KhuyenMaiPrivateUserVIP = "Khuyến mãi USER VIP";
        public const string MainGame = "3Cay";
        public const string BCST = "Bắn Cá Săn Thưởng";
        public const string BC3D = "Bắn Cá 3D";
        public const string BCV = "Bắn Cá Vàng";
        public const string LogIn = "Đăng nhập";
        public const string SignIn = "Đăng ký";
        public const string ChangeCard = "Đổi thưởng thẻ cào";
        public const string ChangeVnKash = "Đổi thưởng VnKash";
        public const string PlayGame = "Đổi thưởng VnKash";
        public const string ChangeBaoKim = "Đổi thưởng BaoKim";
        public const string ChangeMTop = "Đổi thưởng MTop";
        public const string ChangeBank = "Đổi thưởng Ngân hàng";
        public const string TransferGold = "Chuyển xu";
        public const string TopBanner = "Top Banner";
        public const string SliderHomePage = "Slider trang chủ";
        public const string ArticleSlider = "Slider bài viết";
        public const string Sliderbcst = "Slider BCST";
        public const string PopupEvent = "PopupEvent";
        public const string PopupEventBCST = "PopupEventBCST";
        public const string Hide = "Ẩn";
        public const string Show = "Hiển thị";
        public const string Delete = "Xóa";
        public const string EventIsNotBegin = "Event chưa bắt đầu";
        public const string EventIsEnd = "Event đã kết thúc";
        public const string ErrorWhenAddGold = "Cộng gold không thành công";
        public const string UserNotinEvent = "User không tham gia trong event";
        public const string TLMN = "Tiến lên miền nam";
        public const string Binh = "Binh";
        public const string OanTuTi = "Oẳn tù tì";
        public const string U = "Ù";
        public const string DanhChan = "Đánh chắn";
        public const string CoTuong = "Cờ Tướng";
        public const string XiTo = "Xì Tố";
        public const string DeclineInviteFriend = "Từ chối kết bạn";
        public const string SentInviteFriend = "Đã gửi kết bạn";
        public const string AcceptInviteFriend = "Đồng ý kết bạn";
        public const string SentNotConfirm = "Đã gửi nhưng chưa trả lời";
        public const string SqlError = "Lỗi trong sql";
        public const string InviteFriend = "Lời mời kết bạn";
        public const string InviteAssociation = "Lời mời gia nhập bang";

        public const string GetErrorMessage00 = "Không có lỗi";
        public const string GetErrorMessage99 = "Lỗi không được định nghĩa hoặc không rõ nguyên nhân";
        public const string GetErrorMessage01 = "Lỗi tại NgânLượng.vn nên không sinh được phiếu thu hoặc giao dịch";
        public const string GetErrorMessage02 = "Địa chỉ IP của merchant gọi tới NganLuong.vn không được chấp nhận";

        public const string GetErrorMessage03 =
            "Sai tham số gửi tới NganLuong.vn (có tham số sai tên hoặc kiểu dữ liệu)";

        public const string GetErrorMessage04 = "Tên hàm API do merchant gọi tới không hợp lệ (không tồn tại)";
        public const string GetErrorMessage05 = "Sai version của API";
        public const string GetErrorMessage06 = "Mã merchant không tồn tại hoặc chưa được kích hoạt";
        public const string GetErrorMessage07 = "Sai mật khẩu của merchant";
        public const string GetErrorMessage08 = "Tài khoản người bán hàng không tồn tại";
        public const string GetErrorMessage09 = "Tài khoản người nhận tiền đang bị phong tỏa";
        public const string GetErrorMessage10 = "Hóa đơn thanh toán không hợp lệ";
        public const string GetErrorMessage11 = "Số tiền thanh toán không hợp lệ";
        public const string GetErrorMessage12 = "Đơn vị tiền tệ không hợp lệ";
        public const string GetErrorMessage13 = "Sai số lượng sản phẩm";
        public const string GetErrorMessage14 = "Tên sản phẩm không hợp lệ";
        public const string GetErrorMessage15 = "Sai số lượng sản phẩm/hàng hóa trong chi tiết đơn hàng";
        public const string GetErrorMessage16 = "Số tiền trong chi tiết đơn hàng không hợp lệ";
        public const string GetErrorMessage17 = "Phương thức thanh toán không được hỗ trợ";
        public const string GetErrorMessage18 = "Tài khoản hoặc mật khẩu NL của người thanh toán không chính xác";

        public const string GetErrorMessage19 =
            "Tài khoản người thanh toán đang bị phong tỏa, không thể thực hiện giao dịch";

        public const string GetErrorMessage20 = "Số dư khả dụng của người thanh toán không đủ thực hiện giao dịch";
        public const string GetErrorMessage21 = "Giao dịch NL đã được thanh toán trước đó, không thể thực hiện lại";

        public const string GetErrorMessage22 =
            "Ngân hàng từ chối thanh toán (do thẻ/tài khoản ngân hàng bị khóa hoặc chưa đăng ký sử dụng dịch vụ IB)";

        public const string GetErrorMessage23 =
            "Lỗi kết nối tới hệ thống Ngân hàng (NH không trả lời yêu cầu thanh toán)";

        public const string GetErrorMessage24 = "Thẻ/tài khoản hết hạn sử dụng";
        public const string GetErrorMessage25 = "Thẻ/Tài khoản không đủ số dư để thanh toán";
        public const string GetErrorMessage26 = "Nhập sai tài khoản truy cập Internet-Banking";
        public const string GetErrorMessage27 = "Nhập sai OTP quá số lần quy định";

        public const string GetErrorMessage28 =
            "Lỗi phía Ngân hàng xử lý giao dịch thanh toán nhưng chưa rõ nguyên nhân hoặc lỗi này chưa được mô tả";

        public const string GetErrorMessage29 = "Mã token không tồn tại";
        public const string GetErrorMessage30 = "Giao dịch không tồn tại";

        public const string GetErrorMessageSms00 = "Giao dịch thành công";

        public const string GetErrorMessageSms99 =
            "Lỗi tuy nhiên lỗi chưa được định nghĩa hoặc chưa xác định được nguyên nhân";

        public const string GetErrorMessageSms01 = "Lỗi, địa chỉ IP truy cập API của NgânLượng.vn bị từ chối";
        public const string GetErrorMessageSms02 = "Lỗi, tham số gửi từ merchant tới NgânLượng.vn chưa chính xác.";

        public const string GetErrorMessageSms03 =
            "Lỗi, mã merchant không tồn tại hoặc merchant đang bị khóa kết nối tới NgânLượng.vn";

        public const string GetErrorMessageSms04 = "Lỗi, mã checksum không chính xác";
        public const string GetErrorMessageSms05 = "Tài khoản nhận tiền nạp của merchant không tồn tại";

        public const string GetErrorMessageSms06 =
            "Tài khoản nhận tiền nạp của  merchant đang bị khóa hoặc bị phong tỏa, không thể thực hiện được giao dịch nạp tiền";

        public const string GetErrorMessageSms07 = "Thẻ đã được sử dụng";
        public const string GetErrorMessageSms08 = "Thẻ bị khóa";
        public const string GetErrorMessageSms09 = "Thẻ hết hạn sử dụng";
        public const string GetErrorMessageSms10 = "Thẻ chưa được kích hoạt hoặc không tồn tại";
        public const string GetErrorMessageSms11 = "Mã thẻ sai định dạng";
        public const string GetErrorMessageSms12 = "Sai số serial của thẻ";
        public const string GetErrorMessageSms13 = "Mã thẻ và số serial không khớp";
        public const string GetErrorMessageSms14 = "Thẻ không tồn tại";
        public const string GetErrorMessageSms15 = "Thẻ không sử dụng được";
        public const string GetErrorMessageSms16 = "Số lần tưử của thẻ vượt quá giới hạn cho phép";
        public const string GetErrorMessageSms17 = "Hệ thống Telco bị lỗi hoặc quá tải, thẻ chưa bị trừ";

        public const string GetErrorMessageSms18 =
            "Hệ thống Telco  bị lỗi hoặc quá tải, thẻ có thể bị trừ, cần phối hợp với nhà mạng để đối soát";

        public const string GetErrorMessageSms19 = "Kết nối NgânLượng với Telco bị lỗi, thẻ chưa bị trừ.";

        public const string GetErrorMessageSms20 =
            "Kết nối tới Telco thành công, thẻ bị trừ nhưng chưa cộng tiền trên NgânLượng.vn";

        public const string GetErrorMessageSms21 =
            "Lưu ý: Chỉ chấp nhận thẻ các mệnh giá {0}, các mệnh giá khác sẽ không hoàn lại và không được cộng xu";

        public const string
            GetErrorMessageSms22 =
                "Thẻ không được phép nạp vào game do nạp sai quá {0} lần liên tiếp"; //"Bạn đã nạp sai {0} lần trong {1} phút, vui lòng đợi trong giây lát để đến lần nạp tiếp theo";

        public const string GetErrorMessageSms23 = "Loại thẻ bạn nạp không được hỗ trợ";
        public const string GetErrorMessageSms24 = "Bạn đã chọn sai mệnh giá thẻ cào. Vui lòng liên hệ CSKH để hổ trợ!";
        public const string GetErrorMessageSms25 = "Bạn không được phép nạp vào game do nạp sai quá {0} lần liên tiếp";

        public const string DownloadSoftpinLoginFail = "Đăng nhập không thành công";
        public const string DownloadSoftpinCallServiceFail = "Gọi service không thành công";
        public const string EpayGetCardInfoFail = "Lấy thông tin thẻ không thành công";
        public const string EpayGetPinOrResiInfoFail = "Lấy thông tin pin & seri thẻ không thành công";
        public const string LogCardTransDesWating = "Đang chờ kết quả từ đối tác";
        public const string LogCardTransInsertFail = "Lỗi ghi nhận giao dịch. Vui lòng thử lại sau!";
        public const string LogCardTransInsertTimeOut = "Giao dịch đã hết hạn. Vui lòng liên hệ Admin hỗ trợ";
        public const string LogCardTransInsertError = "Lỗi trong quá trình nạp thẻ. Vui lòng thử lại sau (Code=1)!";
        public const string LogCardTransChargeErrorCode2 = "Lỗi trong quá trình nạp thẻ. Vui lòng thử lại sau (Code=2)";
        public const string LogCardTransChargeErrorCode3 = "Lỗi trong quá trình nạp thẻ. Vui lòng thử lại sau (Code=3)";

        public const string LogCardTransChargeWithCallback =
            "Thẻ sẽ được nạp từ 2 - 60 phút. Sau đó xu sẽ được tự động cộng vào tài khoản của bạn";

        public const string LogCardTransNapGoldTheCao = "Nạp gold qua thẻ cào";

        public const string LogCardTransNapGoldSuccess =
            "Nạp thành công thẻ {0} VNĐ. Bạn được tặng <strong> {1} </strong> Xu vào tài khoản";

        public const string LogCardTransNapGoldSuccessNoHtml =
            "Nạp thành công thẻ {0} VNĐ. Bạn nhận được {1} Xu vào tài khoản";

        public const string LogCardTransNapTheTichCode =
            "Bạn được tặng {0} code tham gia xổ số may mắn cho thẻ nạp {1} {2} K. Mã số code của bạn là: {3} . Chúng tôi sẽ tổng kết tất cả code bạn nhận được vào cuối chương trình trước khi xổ số bắt đầu!";

        public const string SaveLogVipPointLog = "Mua Gold đại lý: {0}";
        public const string LoiTruGoldGame = "lỗi khi trừ gold";
        public const string TopBangHoiUserLogTypeEnumUnKnow = "Không Xác Định";
        public const string TopBangHoiUserLogTypeEnumBangHoi = "Bang hội";
        public const string TopBangHoiUserLogTypeEnumCaNhan = "Cá Nhân";
        public const string WalletError = "Không xác định được phương thức bảo vệ Ví";
        public const string WalletTypeError = "Không xác định được loại giao dịch";

        public const string WalletUserLogTypeEnumDoiMatKhau = "Đổi mật khẩu ví ngân hàng";
        public const string WalletUserLogTypeEnumThayDoiSDT = "Đổi số điện thoại OTP ví ngân hàng";
        public const string WalletUserLogTypeEnumTaoMatKhauOtp = "Cấp mật khẩu OTP mới cho ví ngân hàng";
        public const string WalletUserLogTypeEnumDoiMatKhauKhoaVi = "Đổi mật khẩu khóa ví ngân hàng";
        public const string WalletUserLogTypeEnumKhoaVi = "Khóa ví ngân hàng";
        public const string WalletUserLogTypeEnumMoKhoaVi = "Mở khóa ví ngân hàng";
        public const string BanCa = "Bắn Cá Săn Thưởng";
        public const string VQXS = "Vòng quay";
        public const string ShopOnline = "Shop Online";
        public const string FeeOfflineMessage = "Trừ xu phí inbox";
        public const string OfflineMessage = "Offline Message";


        public const string Fail = "Thất bại"; //Thất bại
        public const string Skill = "Kỹ năng"; //Kỹ năng
        public const string ConfigFail = "Cấu hình không tồn tại"; //Cấu hình không tồn tại
        public const string EnoughNotCoins = "Bạn không đủ tiền để mua hàng"; //Bạn không đủ tiền để mua hàng
        public const string InvalidNumber = "Số lượng không hợp lệ"; //Số lượng không hợp lệ
        public const string InvalidMoneyType = "Loại tiền không hợp lệ"; //Loại tiền không hợp lệ

        public const string
            ContinueBuying =
                "Bạn phải sử dụng Item này để tiếp tục mua hàng"; //Bạn phải sử dụng Item này để tiếp tục mua hàng

        public const string
            NotHaveExhausted = "Bạn đã hết số lần mua Item trong ngày"; //Bạn đã hết số lần mua Item trong ngày

        public const string TheNumberIsTooLimited = "Số lượng quá giới hạn"; //Số lượng quá giới hạn

        public const string
            ToBuyANewItem =
                "Để mua item mới bạn vui lòng sử dụng hết item trong túi đồ"; //Để mua item mới bạn vui lòng sử dụng hết item trong túi đồ

        public const string YouHaveRunOutOfTimes = "Bạn đã hết số lần mua Item"; // Bạn đã hết số lần mua Item
        public const string NoData = "Không có dữ liệu"; //Không có dữ liệu
        public const string EventEnds = "Sự kiện đã hết";

        public const string
            YouMustHaveATurn =
                "Bạn phải có tối thiểu 01 lượt quay và {0:#,##0} xu mới được tham gia"; //Bạn phải có tối thiểu 01 lượt quay và {0:#,##0} xu mới được tham gia

        public const string
            MustWaitMoreDays = "Bạn phải đợi {0} ngày nữa mới được quay"; //Bạn phải đợi {0} ngày nữa mới được quay

        public const string
            ToDayHasBeenAwarded = "Giải thưởng hôm nay đã được trao hết"; //Giải thưởng hôm nay đã được trao hết

        public const string HasBeenAwarded = "Giải thưởng đã được trao"; //Giải thưởng đã được trao

        public const string
            CSKHService = "Vui lòng liên hệ với CSKH để được hỗ trợ"; //Vui lòng liên hệ với CSKH để được hỗ trợ

        public const string VipNotRunning = "Chưa chạy vip"; //Chưa chạy vip
        public const string AccountNotVip = "Tài khoản chưa có vip"; //Tài khoản chưa có vip

        public const string
            YourAccountAwarded =
                "Bạn được thưởng {0} vào tài khoản của bạn"; //Bạn được thưởng {0} vào tài khoản của bạn

        public const string
            YouPromotioned =
                "Bạn được khuyến mãi {0}% dựa trên giá trị của thẻ"; //Bạn được khuyến mãi {0}% dựa trên giá trị của thẻ

        public const string GameNotConfig = "Game chưa được cấu hình"; //Game chưa được cấu hình
        public const string GetDataFail = "Lấy dữ liệu thất bại"; //Lấy dữ liệu thất bại
        public const string InvalidLocationId = "Location không hợp lệ"; //Location không hợp lệ
        public const string InvalidChannel = "Channel không hợp lệ"; //Channel không hợp lệ
        public const string UserNotEnoughWatch = "User đã quá giớn hạn số lần coi"; //User đã quá giớn hạn số lần coi
        public const string GetDataSuccess_v1 = "Lấy dữ liệu thành công"; //Lấy dữ liệu thành công
        public const string Orther = "Khác"; //Khác
        public const string Bullet = "Đạn"; //Đạn
        public const string Guns = "Súng"; //Súng
        public const string Effect = "Hiệu ứng"; //Hiệu ứng
        public const string NoItem = "Không có Item";
        public const string ItemCannotActive = "Item không thể kích hoạt";

        public const string Itemdoesnotexist = "Item không tồn tại";
        public const string Buyitemssuccessfully = "Mua Item thành công";
        public const string VipLevelUp = "Nhận thưởng lên level vip thành công"; //Nhận thưởng lên level vip thành công
        public const string Norewards = "Không có phần thưởng để nhận"; //Không có phần thưởng để nhận
        public const string Getrewardedfailure = "Nhận thưởng thất bại"; //Nhận thưởng thất bại
        public const string Norewardfound = "Không tìm thấy phần thưởng để nhận"; //Không tìm thấy phần thưởng để nhận
        public const string Pointstoachieve = "Điểm cần đạt được"; //Điểm cần đạt được
        public const string Expshotfish = "Exp băn cá"; //Exp băn cá
        public const string Accumulatedskill = "Tích lũy tuyệt chiêu"; //Tích lũy tuyệt chiêu
        public const string Transfercoins = "Chuyển xu"; //Chuyển xu
        public const string Getacointransfer = "Nhận chuyển xu"; //Nhận chuyển xu
        public const string Donatedtimes1 = "Tặng lần 1"; //Tặng lần 1
        public const string Donatedtimes2 = "Tặng lần 2"; //Tặng lần 2
        public const string Expcharacter = "Exp nhân vật"; //Exp nhân vật
        public const string Invalidinputdata = "Dữ liệu đầu vào không hợp lệ"; //Dữ liệu đầu vào không hợp lệ
        public const string Levelvipup = "Lên level vip"; // Lên level vip
        public const string Accumulatesuccesspoints = "Tích lũy điểm thành công"; // Tích lũy điểm thành công

        public const string
            Failuretoaccumulatepointsfailed = "Lỗi tích điểm không thành công"; //Lỗi tích điểm không thành công

        public const string
            Youareawardedtoyouraccount = "Bạn đã được thưởng {0} vào tài khoản"; //Bạn đã được thưởng xnxx vào tài khoản

        public const string
            Addingvipuserinformationfailed = "Thêm thông tin vip user thất bại"; //Thêm thông tin vip user thất bại

        public const string Successfullyupdated = "Cập nhật thông tin thành công"; // Cập nhật thông tin thành công
        public const string Emailalreadyexists = "Email của bạn đã tồn tại"; // Email của bạn đã tồn tại
        public const string Accountdoesnotexist = "Tài khoản không tồn tại"; // Tài khoản không tồn tại
        public const string PleaseComeBackTomorrow = "Bạn vui lòng trở lại vào ngày mai.";
        public const string UserReceivedAReward = "User đã nhận thưởng";
        public const string BonusDailyDay = "Ngày";
        public const string Bronze = "Đồng";
        public const string Silver = "Bạc";
        public const string Ruby = "Hồng ngọc";
        public const string Emerald = "Ngọc lục bảo";
        public const string Diamond = "Kim cương";
        public const string Gold = "Vàng"; // Vàng
        public const string EventDoesNotExist = "Sự kiện không tồn tại"; // Sự kiện không tồn tại
        public const string ChannelInvalid = "Channel không hợp lệ"; // Channel không hợp lệ
        public const string UserLimitViews = "User đã quá giới hạn số lần coi"; // User đã quá giới hạn số lần coi

        #region Reason

        public const string Reason_2 = "Nạp IAP";
        public const string Reason_40 = "Mua Item";
        public const string Reason_48 = "Mua Hinh Nen";
        public const string Reason_49 = "Mua avatar";
        public const string Reason_68 = "Tặng Chips khi bang lên Level";
        public const string Reason_70 = "Tặng Chips khi user lên loại thành viên khi nạp Chips";
        public const string Reason_89 = "Tặng Chips Lên Level";
        public const string Reason_102 = "Tặng Chips đăng nhập";
        public const string Reason_105 = "Mua Emotion";
        public const string Reason_108 = "Chuyển Chips cho bạn bè";
        public const string Reason_116 = "Chuyển tiền giữa các thành viên VIP";
        public const string Reason_117 = "Chuyển Tiền Bang Hội cho Thành viên";
        public const string Reason_118 = "Nạp Chips";
        public const string Reason_119 = "Admin tặng Chips";
        public const string Reason_133 = "Tặng Chips online bắn cá";
        public const string Reason_142 = "Khuyến mãi nạp Chips";
        public const string Reason_143 = "Nhận thành tựu";
        public const string Reason_161 = "Khuyến mãi nạp Chips User VIP";
        public const string Reason_162 = "Tặng gold cho user mới trong event thêm bạn them vui";
        public const string Reason_166 = "Tặng Chips đăng ký bằng OpenID";
        public const string Reason_168 = "Tặng Chips event user mới";
        public const string Reason_169 = "Tặng Chips cho user đua top vip";
        public const string Reason_171 = "Tặng Chips hằng ngày";
        public const string Reason_173 = "Admin trừ Chips";
        public const string Reason_174 = "Nạp tiền qua sms";
        public const string Reason_175 = "Đăng nhập app bắn cá";
        public const string Reason_176 = "Thắng Jackpot";
        public const string Reason_178 = "Tặng Chips đăng nhập hàng ngày cho mobile";
        public const string Reason_179 = "Điểm danh nhận thưởng";
        public const string Reason_180 = "Tặng Chips mời bạn bè trên facebook";
        public const string Reason_181 = "Tặng Chips khi bạn bè đăng ký trên facebook";
        public const string Reason_182 = "Khuyến mãi nạp lần đầu";
        public const string Reason_183 = "Tặng Chips khi user đăng nhập liên tục";
        public const string Reason_184 = "Tặng Chips like fanpage trên app fb";
        public const string Reason_187 = "Tặng Chips khi user hết lượt tặng - user mới";
        public const string Reason_188 = "Tặng Chips user mới đăng ký nạp thẻ đầu tiên";
        public const string Reason_189 = "Tặng Chips user đăng nhập mobile giờ vàng";
        public const string Reason_190 = "Tặng Chips đăng nhập ngày vàng Mobile";
        public const string Reason_191 = "Tặng Chips đăng ký từ quảng cáo Yahoo";
        public const string Reason_192 = "Tặng Chips khi cập nhật thông tin";
        public const string Reason_197 = "Khuyến mãi nạp Chips sms";
        public const string Reason_198 = "Khuyến mãi nạp IAP";
        public const string Reason_213 = "Chơi game Bắn Cá";
        public const string Reason_300 = "Tặng Chips Cho User Mới";
        public const string Reason_301 = "Trả Chips Event BONUS TaiXiu";
        public const string Reason_302 = "Đổi Chips";
        public const string Reason_303 = "Đổi thẻ";
        public const string Reason_306 = "Đổi thưởng quay số";
        public const string Reason_307 = "Hệ thống cấp gold cho jackpot";
        public const string Reason_311 = "Nạp tiền qua Google Wallet";
        public const string Reason_312 = "Nạp tiền qua Apple Store";
        public const string Reason_313 = "Tạo bang hội";
        public const string Reason_314 = "Rút";
        public const string Reason_315 = "Nạp";
        public const string Reason_316 = "Bang chủ chuyển tiền cho user";
        public const string Reason_318 = "User kết thúc game bắn cá";
        public const string Reason_319 = "User chơi game bắn cá";
        public const string Reason_320 = "Nhận Chips từ bang chủ";
        public const string Reason_321 = "Tặng 2% doanh số trong tháng của bang cho bang chủ";
        public const string Reason_322 = "Tặng 3% doanh số trong tháng của bang cho người nạp Chips nhiều nhất bang";
        public const string Reason_325 = "Trả Chips cho user sau khi vào phòng chơi không thành công";
        public const string Reason_326 = "Trả Chips cho user sau khi admin hủy yêu cầu đổi thưởng";
        public const string Reason_327 = "Thưởng săn cá boss";
        public const string Reason_328 = "Tặng Chips cho user Event đăng nhập giờ vàng";
        public const string Reason_329 = "Tặng Chips cho user Event hoàn tiền";
        public const string Reason_330 = "Tặng Chips user đăng ký game bắn cá săn thưởng";
        public const string Reason_331 = "Tặng Chips user đăng nhập game Bắn cá săn thưởng";
        public const string Reason_332 = "Tặng Chips khi ra phòng chờ bắn cá";
        public const string Reason_333 = "Nhận thưởng nhiệm vụ hằng ngày";
        public const string Reason_334 = "Thưởng share facebook";
        public const string Reason_335 = "Hệ thống thưởng Chips jackpot";
        public const string Reason_336 = "Hoàn tiền do đổi thẻ không thành công";
        public const string Reason_337 = "Tặng tiền hoàn thành nhiệm vụ Bắn Cá";
        public const string Reason_338 = "Tặng tiền lên level Bắn Cá";
        public const string Reason_342 = "User chuyển tiền cho user khác";
        public const string Reason_343 = "User nhận tiền từ user khác chuyển qua";
        public const string Reason_344 = "Admin trả Chips Bắn Cá";
        public const string Reason_345 = "Admin trả Chips Lather";
        public const string Reason_347 = "Thưởng tổng kết bang hội";
        public const string Reason_350 = "Tặng Chips vòng quay trong game";
        public const string Reason_351 = "Trúng thưởng Game Xóc Rùa";
        public const string Reason_352 = "Thắng sự kiện nhập mã được tiền";
        public const string Reason_353 = "Thắng game đấu trường";
        public const string Reason_355 = "Chơi bắn cá 3D";
        public const string Reason_356 = "Kết thúc bắn cá 3D";
        public const string Reason_357 = "Chơi bắn cá 3D mobile";
        public const string Reason_358 = "Kết thúc bắn cá 3D mobile";
        public const string Reason_359 = "Nhận thưởng nhiệm vụ hằng ngày";
        public const string Reason_404 = "Chơi game Mini Xeng";
        public const string Reason_406 = "Chơi game Mini Penalty";
        public const string Reason_407 = "Khuyến Mãi Giờ Vàng";
        public const string Reason_408 = "Chơi game Mini Fortunes88";
        public const string Reason_409 = "Chơi game Jackpot";
        public const string Reason_410 = "Chơi game XengLuxury";
        public const string Reason_412 = "Chơi game Rồng Vàng Slot";
        public const string Reason_413 = "Chơi Game Tam Quốc Slot";
        public const string Reason_418 = "Chơi game Thần Thú";
        public const string Reason_419 = "Chơi game Thần Tài";
        public const string Reason_420 = "Chơi game Pachinko Robot";
        public const string Reason_422 = "Chơi game Samurai";
        public const string Reason_501 = "Thắng Vòng Quay May Mắn";
        public const string Reason_502 = "Mua Lượt Vòng Quay May Mắn";
        public const string Reason_503 = "Choi Game Lather";
        public const string Reason_999 = "Log phát sinh do IdReason ko tồn tại";

        #endregion

        #endregion
    }
}