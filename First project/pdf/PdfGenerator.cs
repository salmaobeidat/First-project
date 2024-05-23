using iTextSharp.text.pdf;
using iTextSharp.text;

namespace First_project.pdf
{
    public class PdfGenerator
    {
        public static void GeneratePdfOrder(IWebHostEnvironment hostingEnvironment, PdfModel pdf)
        {
            string pdfPath = Path.Combine(hostingEnvironment.WebRootPath, "PDF", "recipe_pdf.pdf");

            using (FileStream fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (Document doc = new Document())
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                    doc.Open();
                    string recipeImagePath = Path.Combine(hostingEnvironment.WebRootPath, "Images", pdf.RecipeImg);
                    if (!string.IsNullOrEmpty(recipeImagePath) && File.Exists(recipeImagePath))
                    {
                        Image recipeImage = Image.GetInstance(recipeImagePath);
                        recipeImage.ScaleToFit(120, 120);

                        PdfTemplate mask2 = writer.DirectContent.CreateTemplate(175, 175);
                        mask2.Ellipse(0, 0, 175, 175);
                        mask2.Clip();
                        mask2.NewPath();
                        mask2.AddImage(recipeImage, 175, 0, 0, 175, 0, 0);

                        Image clippedImage2 = Image.GetInstance(mask2);
                        clippedImage2.Alignment = Image.LEFT_ALIGN;
                        doc.Add(clippedImage2);
                        doc.Add(new Paragraph("\n"));
                    }

                    doc.AddAuthor(pdf.ChefName);
                    doc.Add(new Paragraph($"Recipe Name: {pdf.RecipeName}"));
                    doc.Add(new Paragraph("\n"));

                    doc.Add(new Paragraph($"Description: {pdf.Description}"));
                    doc.Add(new Paragraph("\n"));

                    doc.Add(new Paragraph($"Category Name: {pdf.CategoryName}"));
                    doc.Add(new Paragraph("\n"));

                    doc.Add(new Paragraph($"Ingredients: {pdf.Ingredients}"));
                    doc.Add(new Paragraph("\n"));
                  
                    doc.Add(new Paragraph($"Price: $ {pdf.Price}"));
                    doc.Close();
                }
            }
        }

    }
}




