/// <summary>
/// Author: QuocTuan
/// CreateDate: 2019-04-25
/// Description: Custom Attribute validate
/// </summary>

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ResxLanguagesUtility.Enums;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace ResxLanguagesUtility.Helper
{
    public class RxLangRequiredAttribute : RequiredAttribute
    {
        private readonly string resourceKeyName;
        private readonly ResxLanguagesEnum resxLanguagesEnum;

        public RxLangRequiredAttribute(string _resourceKeyName, ResxLanguagesEnum _resxLanguagesEnum)
        {
            resourceKeyName = _resourceKeyName;
            resxLanguagesEnum = _resxLanguagesEnum;
        }

        public override string FormatErrorMessage(string name)
        {
            return ResxLanguages.GetText(resourceKeyName, resxLanguagesEnum);
        }
    }

    public class RxLangRegularExpressionAttribute : RegularExpressionAttribute
    {
        private string pattern;
        private readonly string resourceKeyName;
        private readonly ResxLanguagesEnum resxLanguagesEnum;

        public RxLangRegularExpressionAttribute(string _pattern, string _resourceKeyName,
            ResxLanguagesEnum _resxLanguagesEnum)
            : base(_pattern)
        {
            resourceKeyName = _resourceKeyName;
            resxLanguagesEnum = _resxLanguagesEnum;
            pattern = _pattern;
        }

        public override string FormatErrorMessage(string name)
        {
            return ResxLanguages.GetText(resourceKeyName, resxLanguagesEnum);
        }
    }

    public class RxLangStringLengthAttribute : StringLengthAttribute
    {
        private int maximumLength;
        private readonly string resourceKeyName;
        private readonly ResxLanguagesEnum resxLanguagesEnum;

        public RxLangStringLengthAttribute(int _maximumLength, string _resourceKeyName,
            ResxLanguagesEnum _resxLanguagesEnum)
            : base(_maximumLength)
        {
            resourceKeyName = _resourceKeyName;
            resxLanguagesEnum = _resxLanguagesEnum;
            maximumLength = _maximumLength;
        }

        public int RxLangMaximumLength => MaximumLength;

        public int RxLangMinimumLength
        {
            get => MinimumLength;
            set => MinimumLength = value;
        }

        public override string FormatErrorMessage(string name)
        {
            return ResxLanguages.GetText(resourceKeyName, resxLanguagesEnum);
        }
    }

    public class RxLangRemoteAttribute : RemoteAttribute
    {
        private string action, controller;
        private readonly string resourceKeyName;
        private readonly ResxLanguagesEnum resxLanguagesEnum;

        public RxLangRemoteAttribute(string _action, string _controller, string _resourceKeyName,
            ResxLanguagesEnum _resxLanguagesEnum)
            : base(_action, _controller)
        {
            resourceKeyName = _resourceKeyName;
            resxLanguagesEnum = _resxLanguagesEnum;
            action = _action;
            controller = _controller;
        }

        public string RxLangHttpMethod
        {
            get => HttpMethod;
            set => HttpMethod = value;
        }

        public string RxLangAdditionalFields
        {
            get => AdditionalFields;
            set => AdditionalFields = value;
        }

        public override string FormatErrorMessage(string name)
        {
            return ResxLanguages.GetText(resourceKeyName, resxLanguagesEnum);
        }
    }

    public class RxLangRangeAttribute : RangeAttribute
    {
        private object miniMum, maxiMum;
        private readonly string resourceKeyName;
        private readonly ResxLanguagesEnum resxLanguagesEnum;

        public RxLangRangeAttribute(int _minimum, int _maximum, string _resourceKeyName,
            ResxLanguagesEnum _resxLanguagesEnum)
            : base(_minimum, _maximum)
        {
            resourceKeyName = _resourceKeyName;
            resxLanguagesEnum = _resxLanguagesEnum;
            miniMum = _minimum;
            maxiMum = _maximum;
        }

        public RxLangRangeAttribute(double _minimum, double _maximum, string _resourceKeyName,
            ResxLanguagesEnum _resxLanguagesEnum)
            : base(_minimum, _maximum)
        {
            resourceKeyName = _resourceKeyName;
            resxLanguagesEnum = _resxLanguagesEnum;
            miniMum = _minimum;
            maxiMum = _maximum;
        }

        public object RxLangMinimum => Minimum;
        public object RxLangMaximum => Maximum;

        public override string FormatErrorMessage(string name)
        {
            return ResxLanguages.GetText(resourceKeyName, resxLanguagesEnum);
        }
    }

    public class RxLangMaxLengthAttribute : MaxLengthAttribute
    {
        private int length;
        private readonly string resourceKeyName;
        private readonly ResxLanguagesEnum resxLanguagesEnum;

        public RxLangMaxLengthAttribute(string _resourceKeyName, ResxLanguagesEnum _resxLanguagesEnum)
        {
            resourceKeyName = _resourceKeyName;
            resxLanguagesEnum = _resxLanguagesEnum;
        }

        public RxLangMaxLengthAttribute(int _length, string _resourceKeyName, ResxLanguagesEnum _resxLanguagesEnum)
            : base(_length)
        {
            resourceKeyName = _resourceKeyName;
            resxLanguagesEnum = _resxLanguagesEnum;
            length = _length;
        }

        public override string FormatErrorMessage(string name)
        {
            return ResxLanguages.GetText(resourceKeyName, resxLanguagesEnum);
        }
    }

    public class RxLangMinLengthAttribute : MinLengthAttribute
    {
        private int length;
        private readonly string resourceKeyName;
        private readonly ResxLanguagesEnum resxLanguagesEnum;

        public RxLangMinLengthAttribute(int _length, string _resourceKeyName, ResxLanguagesEnum _resxLanguagesEnum)
            : base(_length)
        {
            resourceKeyName = _resourceKeyName;
            resxLanguagesEnum = _resxLanguagesEnum;
            length = _length;
        }

        public override string FormatErrorMessage(string name)
        {
            return ResxLanguages.GetText(resourceKeyName, resxLanguagesEnum);
        }
    }

    public class RxLangCompareAttribute : CompareAttribute
    {
        private string otherProperty;
        private readonly string resourceKeyName;
        private readonly ResxLanguagesEnum resxLanguagesEnum;

        public RxLangCompareAttribute(string _otherProperty, string _resourceKeyName,
            ResxLanguagesEnum _resxLanguagesEnum)
            : base(_otherProperty)
        {
            resourceKeyName = _resourceKeyName;
            resxLanguagesEnum = _resxLanguagesEnum;
            otherProperty = _otherProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return ResxLanguages.GetText(resourceKeyName, resxLanguagesEnum);
        }
    }
}