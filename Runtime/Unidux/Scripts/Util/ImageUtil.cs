using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Unidux.Scripts.Util
{
    public class ImageUtil
    {
        public static Vector2 GetFitImageSize(float sourceWidth, float sourceHeight, float wConstrain, float hConstrain)
        {
            var wCoef = wConstrain / sourceWidth;
            var hCoef = hConstrain / sourceHeight;

            var wbigger = (wCoef < 1 ? 1 / wCoef : wCoef) > (hCoef < 1 ? 1 / hCoef : hCoef);

            Debug.Log($"wbigger {wbigger}");


            if (wbigger)
            {
                return new Vector2(wConstrain, sourceHeight * wCoef);
            }
            else
            {
                return new Vector2(sourceWidth * hCoef, hConstrain);
            }
        }
    }
}
