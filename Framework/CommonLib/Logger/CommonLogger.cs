/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: CommonLogger
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using log4net;
using log4net.Config;

namespace Logger
{
    public class CommonLogger
    {
        private const string DEFAULT_LOGGER = "DefaultLogger";
        private const string PAYMENT_LOGGER = "PaymentLogger";
        private const string PERFORMANCE_LOGGER = "PerformanceLogger";
        private const string MOBILE_LOGGER = "MobileLogger";
        private const string LOCK_LOGGER = "LockLogger";

        static CommonLogger()
        {
            //log4net.Config.DOMConfigurator.Configure();
            XmlConfigurator.Configure();
            DefaultLogger = LogManager.GetLogger(DEFAULT_LOGGER);
            PaymentLogger = LogManager.GetLogger(PAYMENT_LOGGER);
            PerformanceLogger = LogManager.GetLogger(PERFORMANCE_LOGGER);
            MobileLogger = LogManager.GetLogger(MOBILE_LOGGER);
            LockLogger = LogManager.GetLogger(LOCK_LOGGER);
        }

        /// <summary>
        ///     Ghi log chung cho toan project
        /// </summary>
        public static ILog DefaultLogger { get; set; }

        /// <summary>
        ///     Ghi log su dung trong chuc nang Payment
        /// </summary>
        public static ILog PaymentLogger { get; set; }

        /// <summary>
        ///     Ghi log su dung trong chuc nang Payment
        /// </summary>
        public static ILog PerformanceLogger { get; set; }

        /// <summary>
        ///     Ghi log su dung trong chuc nang Payment
        /// </summary>
        public static ILog MobileLogger { get; set; }

        /// <summary>
        ///     Ghi log tai khoan bi lock
        /// </summary>
        public static ILog LockLogger { get; set; }
    }
}