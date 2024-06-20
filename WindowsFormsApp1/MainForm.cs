using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
	public partial class MainForm : Form
	{
		private List<PointF> pointsToDraw = new List<PointF>();

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// Инициализация отмеченного элемента (черный прямоугольник)
			Outline markedElementOutline = new Outline
			{
				Min = new Point3d { X = 5, Y = 5, Z = 0 },
				Max = new Point3d { X = 8, Y = 8, Z = 0 }
			};
			pointsToDraw.Add(new PointF(5 * 10, 5 * 10));
			pointsToDraw.Add(new PointF(8 * 10, 8 * 10));


			// Инициализация искомого элемента (красный прямоугольник)
			Outline targetElementOutline = new Outline
			{
				Min = new Point3d { X = 0, Y = 0, Z = 0 },
				Max = new Point3d { X = 3, Y = 3, Z = 0 }
			};


			// Инициализация препятствий (зеленые прямоугольники)
			List<Outline> anotherObjectOutlines = new List<Outline>
			{
				new Outline
				{
					Min = new Point3d { X = 10, Y = 5, Z = 0 },
					Max = new Point3d { X = 12, Y = 7, Z = 0 }
				},
				new Outline
				{
					Min = new Point3d { X = 12, Y = 12, Z = 0 },
					Max = new Point3d { X = 9, Y = 9, Z = 0 }
				}
			};
			pointsToDraw.Add(new PointF(10 * 10, 5 * 10));
			pointsToDraw.Add(new PointF(12 * 10, 7 * 10));
			pointsToDraw.Add(new PointF(12 * 10, 12 * 10));
			pointsToDraw.Add(new PointF(9 * 10, 9 * 10));


			// Инициализация линий (зеленые линии)
			List<Line> anotherObjectLines = new List<Line>
			{
				new Line
				{
					Start = new Point3d { X = 15, Y = 5, Z = 0 },
					End = new Point3d { X = 15, Y = 10, Z = 0 }
				},
				new Line
				{
					Start = new Point3d { X = 3, Y = 15, Z = 0 },
					End = new Point3d { X = 8, Y = 15, Z = 0 }
				}
			};
			pointsToDraw.Add(new PointF(15 * 10, 5 * 10));
			pointsToDraw.Add(new PointF(15 * 10, 10 * 10));
			pointsToDraw.Add(new PointF(3 * 10, 15 * 10));
			pointsToDraw.Add(new PointF(8 * 10, 15 * 10));


			ElementPlacer placer = new ElementPlacer();

			PlaceDirection direction = placer.ChoosePlaceDirection(markedElementOutline, targetElementOutline, anotherObjectOutlines, anotherObjectLines);

			var target = placer.GetTargetPositionTestVisual();
			pointsToDraw.Add(new PointF((float)target.Min.X * 10, (float)target.Min.Y * 10));
			pointsToDraw.Add(new PointF((float)target.Max.X * 10, (float)target.Max.Y * 10));
			PaintForm(pointsToDraw);

			lblResult.Text = $"Рекомендуемое направление для размещения элемента: {direction}";
		}

		private Label lblResult;

		private void InitializeComponent()
		{
			this.lblResult = new System.Windows.Forms.Label();
			this.SuspendLayout();

			this.lblResult.AutoSize = true;
			this.lblResult.Location = new System.Drawing.Point(13, 13);
			this.lblResult.Name = "lblResult";
			this.lblResult.Size = new System.Drawing.Size(0, 17);
			this.lblResult.TabIndex = 0;

			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 800);
			this.Controls.Add(this.lblResult);
			this.Name = "MainForm";
			this.Text = "Element Placer";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		public void PaintForm(List<PointF> points)
		{
			pointsToDraw = points;

			this.Paint += new PaintEventHandler(PaintForm_Paint);
		}

		private void PaintForm_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Brush brush = Brushes.Black;
			Font font = new Font(FontFamily.GenericSansSerif, 8);

			foreach (PointF point in pointsToDraw)
			{
				Point intPoint = Point.Round(point);

				g.FillEllipse(brush, intPoint.X - 2, intPoint.Y - 2, 5, 5);

				string text = $"({point.X}, {point.Y})";
				SizeF textSize = g.MeasureString(text, font);

				g.DrawString(text, font, Brushes.Black, intPoint.X + 8, intPoint.Y - textSize.Height);
			}
		}
	}
}

