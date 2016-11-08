﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWPF
{
    public enum MouseHandlingMode
    {
        /// <summary>
        /// Not in any special mode.
        /// </summary>
        None,

        /// <summary>
        /// The user is left-dragging rectangles with the mouse.
        /// </summary>
        DraggingRectangles,

        /// <summary>
        /// The user is left-mouse-button-dragging to pan the viewport.
        /// </summary>
        Panning,

        /// <summary>
        /// The user is holding down shift and left-clicking or right-clicking to zoom in or out.
        /// </summary>
        Zooming,
    }


    public enum DeferedStatus
    {
        None =0,
        DeferedLeft = 1,
        DeferedRight = 2,
        DeferedUp = 3,
        DeferedDown = 4
    }
}