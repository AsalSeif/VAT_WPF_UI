using CommonException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTaxApp
{
    
    public class UIBusinessExceptionEnum : BusinessEnum
    {
        protected UIBusinessExceptionEnum(int internalValue) : base(internalValue)
        {

        }
        public static readonly BusinessEnum TaxRateIsNotValid = new BusinessEnum(200, "VAT Rate is not valid.");
        public static readonly BusinessEnum CalculationBaseValueIsNotValid = new BusinessEnum(201, "Price or VAT value is Zero or Negative!!!");
        public static readonly BusinessEnum CountryIsNotSupported = new BusinessEnum(202, "Country is not supported!!!");
        public static readonly BusinessEnum TaxRangeIsNotSpecified= new BusinessEnum(203, "VAT Rate does not specified!!!");
        public static readonly BusinessEnum ServerSideErrors=new BusinessEnum(204,"Server is not reachable or response is not OK! ");
    }
}
