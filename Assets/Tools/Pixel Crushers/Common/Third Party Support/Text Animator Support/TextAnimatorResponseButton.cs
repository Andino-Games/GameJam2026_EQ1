// Copyright (c) Pixel Crushers. All rights reserved.

using System.Collections;
using Febucci.UI;
using UnityEngine;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Use this subclass of StandardUIResponseButton if your response buttons
    /// use Text Animator.
    /// </summary>
    public class TextAnimatorResponseButton : StandardUIResponseButton
    {

        public override string text
        {
            get
            {
                return label.text;
            }
            set
            {
                var textAnimator = label.gameObject.GetComponent<TextAnimator_TMP>();
                if (textAnimator != null) textAnimator.SetText(value);
                else label.text = value;
                UITools.SendTextChangeMessage(label);
            }
        }

    }

}
