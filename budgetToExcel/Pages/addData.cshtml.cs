using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using System;
using System.IO;

namespace budgetToExcel.Pages
{
    public class addDataModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(int income, int expenses, int phonesubscription, decimal taxRate)
        {
            string fileName = "data.xlsx";
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            if (taxRate > 1)
            {
                taxRate = taxRate / 100;
            }

            decimal taxAmount = income * taxRate;
            decimal incomeAfterTax = income - taxAmount;
            decimal difference = incomeAfterTax - expenses - phonesubscription;

            //Budget forslag graf

            decimal foodAllocation = incomeAfterTax * 0.3m;
            decimal entertainmentAllocation = incomeAfterTax * 0.2m;
            decimal savingsAllocation = incomeAfterTax * 0.2m;
            decimal otherExpensesAllocation = incomeAfterTax * 0.3m;


            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                worksheet.Cells[1, 1].Value = "Indt�gter (f�r skat)";
                worksheet.Cells[1, 2].Value = "Indt�gter (efter skat)";
                worksheet.Cells[1, 3].Value = "Udgifter";
                worksheet.Cells[1, 4].Value = "Mobilabonnement";
                worksheet.Cells[1, 5].Value = "Skatteprocent";
                worksheet.Cells[1, 6].Value = "Skat";
                worksheet.Cells[1, 8].Value = "Overskud";

                worksheet.Cells[2, 1].Value = income; 
                worksheet.Cells[2, 2].Value = incomeAfterTax; 
                worksheet.Cells[2, 3].Value = expenses;
                worksheet.Cells[2, 4].Value = phonesubscription;
                worksheet.Cells[2, 5].Value = taxRate * 100; 
                worksheet.Cells[2, 6].Value = taxAmount;
                worksheet.Cells[2, 8].Value = difference;

                // autosizer kollonner, s� det passer til tekstens l�ngde

                for (int col = 1; col <= 8; col++)
                {
                    worksheet.Column(col).AutoFit();
                }

                // gemmer excel filen i den path jeg har sagt den skal, som er directory i dette tilf�lde

                package.SaveAs(new FileInfo(filePath));
            }

            return RedirectToPage("/graph_allocation", new
            {
                FoodAllocation = foodAllocation,
                EntertainmentAllocation = entertainmentAllocation,
                SavingsAllocation = savingsAllocation,
                OtherExpensesAllocation = otherExpensesAllocation,
            });

        }







    }
}