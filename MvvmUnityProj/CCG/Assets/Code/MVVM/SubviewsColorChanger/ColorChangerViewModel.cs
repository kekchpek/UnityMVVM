using System;
using System.Collections.Generic;
using CCG.MVVM.SubviewsColorChanger.Color;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.SubviewsColorChanger
{
    public class ColorChangerViewModel : ViewModel, IColorChangerViewModel
    {

        private const string SubviewName1 = "ColorA";
        private const string SubviewName2 = "ColorB";
        private const string SubviewName3 = "ColorC";
        private const string SubviewName4 = "ColorD";

        private static readonly IReadOnlyList<UnityEngine.Color> Colors = new []
        {
            UnityEngine.Color.red,
            UnityEngine.Color.blue,
            UnityEngine.Color.green,
        };

        private readonly Dictionary<string, int> _subviewsColorsIndices = new()
        {
            {SubviewName1, 0},
            {SubviewName2, 0},
            {SubviewName3, 0},
            {SubviewName4, 0},
        };

        private int _subviewToChange;

        public void ChangeColor()
        {
            var subviewName = (_subviewToChange++ % 4) switch
            {
                0 => SubviewName1,
                1 => SubviewName2,
                2 => SubviewName3,
                3 => SubviewName4,
                _ => throw new ArgumentOutOfRangeException()
            };
            var subview = GetSubview<IColorViewHandle>(subviewName);
            subview.SetColor(Colors[_subviewsColorsIndices[subviewName]]);
            _subviewsColorsIndices[subviewName] = (_subviewsColorsIndices[subviewName] + 1) % Colors.Count;
        }
    }
}