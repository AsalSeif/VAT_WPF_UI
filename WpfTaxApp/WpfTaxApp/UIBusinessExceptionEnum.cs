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
        public static readonly BusinessEnum TaxRateIsNotValid = new BusinessEnum(100, "VAT Rate is not valid.");
        public static readonly BusinessEnum CalculationBaseValueIsNotValid = new BusinessEnum(101, "Price or VAT value is Zero or Negative!!!");
        public static readonly BusinessEnum CountryIsNotSupported = new BusinessEnum(102, "Country is not supported!!!");
        public static readonly BusinessEnum TaxRangeIsNotSpecified= new BusinessEnum(103, "VAT Rate does not specified!!!");
        public static readonly BusinessEnum ServerSideErrors=new BusinessEnum(104,"ServerSide errors ");
    }
}
