using System;

namespace CommonException
{
    public class BusinessException : Exception
    {
        public BusinessException(BusinessEnum bussinesExceptionCode)
            : base(bussinesExceptionCode.Description)
        {
            BussinesExceptionCode = bussinesExceptionCode;
        }
        public BusinessException(string message, BusinessEnum bussinesExceptionCode)
            : base(message)
        {
            BussinesExceptionCode = bussinesExceptionCode;
        }

        public BusinessEnum BussinesExceptionCode { get; set; }
        public BusinessException(string message, Exception innerException, BusinessEnum bussinesExceptionCode)
           : base(message, innerException)
        {
            BussinesExceptionCode = bussinesExceptionCode;
        }
    }

    public class BusinessEnum
    {
        public static readonly BusinessEnum NotDefined = new BusinessEnum(0);

        public int InternalValue { get; protected set; }
        public string Description { get; protected set; }
        public BusinessEnum(int internalValue, string description = null)
        {
            this.InternalValue = internalValue;
            Description = description;
        }
    }
}
