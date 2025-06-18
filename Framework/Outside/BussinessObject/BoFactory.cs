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
    
        public static TanTamBo Vip => new TanTamBo();
        public static AuthBo Auth => new AuthBo();
    }
}