using MetroFramework.Forms;

public class CustomMetroForm : MetroForm
{
    // Define a class-level variable to hold the custom color
    private Color _customLineColor;

    public CustomMetroForm()
    {
        // Disable the default title text rendering
        this.Text = "";  // Disable default title text rendering

        // Set the default style (optional, as a base style)
        this.Style = MetroFramework.MetroColorStyle.White;

        // Define the custom color (RGB) for the top line in the constructor
        _customLineColor = Color.FromArgb(0, 157, 128);  // Custom RGB color
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // Use the custom color defined in the constructor
        using (Pen linePen = new Pen(_customLineColor, 10))  // Adjust thickness as needed
        {
            // Draw the line at the top of the form (spanning the width of the form)
            e.Graphics.DrawLine(linePen, 0, 0, this.Width, 0);  // Line on top of the form
        }

        // Define the custom font for the title (e.g., 12px size)
        Font customTitleFont = new Font("Segoe UI", 12, FontStyle.Bold);

        // Draw the custom title in the desired font and position
        e.Graphics.DrawString("TechBOM v3.1", customTitleFont, Brushes.Black, new PointF(20, 8));  // Adjust position

        // Clean up the font object after use
        customTitleFont.Dispose();
    }
}
