
------------ Cấu hình, Quy tắc đặt file, thư mục -------------------
1. Cấu hình đường dẫn thư mục - tính từ thư mục gốc
/// Description: Nếu không cấu hình thì folder hoặc vitual folder chứa languages mặc định là ResxLanguages

<DefaultResxLanguagesUtility DefaultDirectory="ResxLanguages" />

/// Cấu hình app setting (trường hợp k tìm thấy url)
AppSettings["RESX_LANGUAGE_PATH"]

2. Quy tắc đặt tên file

Folder: ResxLanguages => Folder module => file xml các ngôn ngữ 
/// Description: Lưu ý Folder module và file xml phải trùng tên

VD: Account/Account.vi.xml
VD: Account/Account.en.xml

----------- Hướng dẫn sử dụng -----------------------
B1: Add LanguagesUtility.dll vào project
B2: 
using ResxLanguagesUtility;
using ResxLanguagesUtility.Enums;

-------- Function
1. Lấy ngôn ngữ hiện tại
/// Description: Là 1 property lấy ra ngôn ngữ hiện tại user đang chọn, thứ tự ưu tiên Cookie, culture, myconfig
/// Response: string mã ngôn ngữ hiện tại
string currentLang = ResxLanguages.GetCurrentLanguage;

2. Sử dụng hàm GetText()
/// Description: Lấy giá trị trong file xml với language keyName
/// <param name="keyName"> Là key định nghĩa trong file xml</param>
/// <param name="ResxLanguagesEnum"> Là 1 enum được định nghĩa sẵn tham khảo ResxLanguagesEnum</param>
/// Response: trả về empty nếu key không tồn tại hoặc có lỗi xảy ra

string text = ResxLanguages.GetText("AcceptRule", ResxLanguagesEnum.Common);

3. Sử dụng hàm GetKey()
/// Description: Lấy keyName theo 1 giá trị văn bản - nếu là 1 list key thì chỉ trả về key đầu tiên
/// <param name="textFind"> Là văn bản cần tìm key trong file xml</param>
/// <param name="ResxLanguagesEnum"> Là 1 enum được định nghĩa sẵn tham khảo ResxLanguagesEnum</param>
/// Response: trả về empty nếu key không tồn tại hoặc có lỗi xảy ra

string key = ResxLanguages.GetKey("Tài khoản EN", ResxLanguagesEnum.Common);

----------------

ResxLanguages
{
        Account = 1,
        Cashout = 2,
        Common = 3,
        Event = 4,
        Game = 5,
        Guild = 6,
        Home = 7,
        Payment = 8,
        Reason = 9,
        Other = 100
}