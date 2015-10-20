using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomOpenCards.Converter
{
    public class HeightWidthConvter
    {
        private static HeightWidthConvter _instance = new HeightWidthConvter();
        public static HeightWidthConvter Instance
        {
            get
            {
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public double PackageHeight { get; set; }
        public double PackageWdith { get; set; }
        public double PackageLeft { get; set; }
        public double PackageTop { get; set; }

        public double CardSlotHeight { get; set; }
        public double CardSlotWdith { get; set; }
        public double CardSlotLeft { get; set; }
        public double CardSlotTop { get; set; }

        public double BackBtnHeight { get; set; }
        public double BackBtnWdith { get; set; }
        public double BackBtnLeft { get; set; }
        public double BackBtnTop { get; set; }

        public double EffectivePosition { get; set; }


        public double MinLeft
        {
            get
            {
                return CardSlotLeft - 200;
            }
        }
        public double MaxLeft
        {
            get
            {
                return CardSlotLeft + CardSlotWdith + 200;
            }
        }

        public double MinTop
        {
            get
            {
                return CardSlotTop - 100;
            }
        }

        public double MaxTop
        {
            get
            {
                return CardSlotTop + CardSlotHeight + 100;
            }
        }


        public void Convter(double OriginalHeight, double OriginalWidth)
        {
            var heightRatio = OriginalHeight / 900;
            var widthRatio = OriginalWidth / 1440;

            PackageLeft = widthRatio * 150;
            PackageTop = heightRatio * 334;
            PackageWdith = widthRatio * 194;
            PackageHeight = heightRatio * 257;


            CardSlotLeft = widthRatio * 722;
            CardSlotTop = heightRatio * 276;
            CardSlotWdith = widthRatio * 221;
            CardSlotHeight = heightRatio * 291;

            BackBtnLeft = widthRatio * 1175;
            BackBtnTop = heightRatio * 800;
            BackBtnWdith = widthRatio * 135;
            BackBtnHeight = heightRatio * 50;
        }
    }
}
