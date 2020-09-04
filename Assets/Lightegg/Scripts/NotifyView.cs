using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotifyView : MonoBehaviour {
   
   [SerializeField] private NotifyViewType _notifyViewType;
   
   private Text _primaryText,_secondaryText; 
   private Button _primaryButton, _secondaryButton;
   
   public void Init() {
      switch (_notifyViewType) {
         case NotifyViewType.Changelog:
            var texts = GetComponentsInChildren<Text>();
            var buttons = GetComponentsInChildren<Button>();
            _primaryText = texts[0];
            _secondaryText = texts[1];
            _primaryButton = buttons[0];
            _secondaryButton = buttons[1];
            break;
         case NotifyViewType.Error:
            _primaryText = GetComponentInChildren<Text>();
            _primaryButton = GetComponentInChildren<Button>();
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }
   
   public void Hide() => gameObject.SetActive(false);

   public void Show() => gameObject.SetActive(true);
   
   public void SetPrimaryText(string str) => _primaryText.text = str;
   
   public void SetSecondaryText(string str) => _secondaryText.text = str;
   
   public void SetPrimaryButtonListener(UnityAction act) => _primaryButton.onClick.AddListener(act);
   
   public void SetSecondaryButtonListener(UnityAction act) => _secondaryButton.onClick.AddListener(act);
   
}

/// <summary>
/// Changelog:TitleText>ContentText>CloseButton>PrivacyButton
/// ErrorView:MessageText>RetryButton
/// LoadingView:NetworkImage>LoadingText>UIProgressBar
/// </summary>
public enum NotifyViewType { Changelog,Error,}