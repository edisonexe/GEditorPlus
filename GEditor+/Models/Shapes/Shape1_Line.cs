﻿using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System.Collections.Generic;
using static GEditor.Models.Shapes.PropsN;

namespace GEditor.Models.Shapes {
    public class Shape1_Line: IShape {
        private static readonly PropsN[] props = new[] { PName, PStartDot, PEndDot, PColor, PThickness };

        public PropsN[] Props => props;

        public string Name => "Линия";



        public Shape? Build(Mapper map) {
            if (map.GetProp(PName) is not string @name) return null;

            if (map.GetProp(PStartDot) is not SafePoint @start || !@start.Valid) return null;

            if (map.GetProp(PEndDot) is not SafePoint @end || !@end.Valid) return null;

            if (map.GetProp(PColor) is not string @color) return null;

            if (map.GetProp(PThickness) is not int @thickness) return null;

            return new Line {
                Name = "sn_" + @name,
                StartPoint = @start.Point,
                EndPoint = @end.Point,
                Stroke = new SolidColorBrush(Color.Parse(@color)),
                StrokeThickness = @thickness
            };
        }
        public bool Load(Mapper map, Shape shape) {
            if (shape is not Line @line) return false;
            if (@line.Name == null || !@line.Name.StartsWith("sn_")) return false;
            if (@line.Stroke == null) return false;

            if (map.GetProp(PStartDot) is not SafePoint @start) return false;
            if (map.GetProp(PEndDot) is not SafePoint @end) return false;

            map.SetProp(PName, @line.Name[3..]);

            @start.Set(@line.StartPoint);
            @end.Set(@line.EndPoint);

            map.SetProp(PColor, ((SolidColorBrush) @line.Stroke).Color.ToString());
            map.SetProp(PThickness, (int) line.StrokeThickness);

            return true;
        }



        public Dictionary<string, object?>? Export(Shape shape) {
            if (shape is not Line @line) return null;
            if (@line.Name == null || !@line.Name.StartsWith("sn_")) return null;

            return new() {
                ["name"] = @line.Name[3..],
                ["start"] = @line.StartPoint,
                ["end"] = @line.EndPoint,
                ["stroke"] = @line.Stroke,
                ["thickness"] = (int) @line.StrokeThickness
            };
        }
        public Shape? Import(Dictionary<string, object?> data) {
            if (!data.ContainsKey("name") || data["name"] is not string @name) return null;

            if (!data.ContainsKey("start") || data["start"] is not Point @start) return null;
            if (!data.ContainsKey("end") || data["end"] is not Point @end) return null;

            if (!data.ContainsKey("stroke") || data["stroke"] is not SolidColorBrush @color) return null;
            if (!data.ContainsKey("thickness") || data["thickness"] is not short @thickness) return null;

            return new Line {
                Name = "sn_" + @name,
                StartPoint = @start,
                EndPoint = @end,
                Stroke = @color,
                StrokeThickness = @thickness
            };
        }
    }
}
