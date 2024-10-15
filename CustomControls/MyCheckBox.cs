
namespace TechBOM.CustomControls
{
    internal class MyCheckBox : CheckBox
    {
        public MyCheckBox()
        {
            // Set the appearance to allow custom drawing
            this.Appearance = Appearance.Normal;
            this.Text = "MyCheckBox";
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            // Clear the background
            pevent.Graphics.Clear(this.BackColor);

            // Define the size and position for the square border
            Rectangle borderRect = new(2, 2, 14, 14);  // Slightly larger for border

            // Draw the border of the square
            Pen borderPen = new(Color.Black, 1);  // Black border, 2px thick
            pevent.Graphics.DrawRectangle(borderPen, borderRect);
            borderPen.Dispose();

            // Define the inner filled square (smaller, inside the border)
            Rectangle innerRect = new(5, 5, 9, 9);  // Smaller to fit inside the border

            // Draw the filled square only when checked
            if (this.Checked)
            {
                SolidBrush fillBrush = new(Color.FromArgb(0, 157, 128)); // Filled color (175, 175, 0)
                pevent.Graphics.FillRectangle(fillBrush, innerRect);
                fillBrush.Dispose();
            }

            // Draw the checkbox text next to the square
            TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font,
                                  new Point(22, 2), this.ForeColor);  // Position text further due to border
        }
    }
}
