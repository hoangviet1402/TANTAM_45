namespace BussinessObject.Enum
{
    public enum Generate_Timekeeping_Type_Obj_Enum
    {
        generate_from_start_of_month = 1
    }

    public enum Shift_Type_Enum
    {
        standard_working = 1, // ca mặc định dùng để tạo theo chu kỳ
        shift_working = 2,
        shift_assignment = 3
        //fixed = 3
    }

    public enum Clock_Type_Enum
    {
        clock_in  = 1,
        clock_out = 2,
        admin = 3
    }

    public enum Connection_Type_Enum
    {
        wifi = 1,
        Gps = 2
    }

    //TimeKeeperDevice
    public enum TimeKeeper_Device_Enum
    {
        mobile = 1,
        web = 2
    }

    public enum Shift_status
    {
        active = 1,
        deactive = 2
    }

    public enum Shift_ActionType_Enum
    {
        checkin = 1,
        checkout = 2,
        uncheckin = 3,
        uncheckout = 4
    }

    public enum Wifi_type_Enum
    {
        wifi = 1,
        Gps = 2
    }

}
