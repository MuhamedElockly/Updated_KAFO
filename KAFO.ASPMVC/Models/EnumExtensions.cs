using KAFO.Domain.Invoices;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace KAFO.ASPMVC.Models
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this InvoiceType invoiceType)
        {
            var fieldInfo = invoiceType.GetType().GetField(invoiceType.ToString());
            var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.Name ?? invoiceType.ToString();
        }
    }
}
