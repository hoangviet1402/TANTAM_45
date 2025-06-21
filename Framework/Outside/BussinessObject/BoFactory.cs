/*********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: BoFactory
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using BussinessObject.Bo.TanTamBo;

namespace BussinessObject
{
    public class BoFactory
    {
        public static AuthBo Auth => new AuthBo();
        public static CompanyBo Company => new CompanyBo();
        public static BranchesBo Branches => new BranchesBo();
        public static DepartmentBo Department => new DepartmentBo();
    }
}