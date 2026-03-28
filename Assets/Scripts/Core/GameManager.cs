using UnityEngine;
using Frezerka.Utility;

namespace Frezerka.Core
{
    public enum SessionMode
    {
        Training,
        Normal
    }

    public enum GameLanguage
    {
        RU,
        EN
    }

    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        [Header("Session Settings")]
        [SerializeField] private string participantId = "student_001";
        [SerializeField] private SessionMode sessionMode = SessionMode.Training;
        [SerializeField] private GameLanguage language = GameLanguage.RU;

        public string ParticipantId
        {
            get => participantId;
            set => participantId = value;
        }

        public SessionMode CurrentMode
        {
            get => sessionMode;
            set => sessionMode = value;
        }

        public GameLanguage CurrentLanguage
        {
            get => language;
            set => language = value;
        }

        public bool IsTrainingMode => sessionMode == SessionMode.Training;

        public string SelectedMachineType { get; set; } = "Lathe";

        protected override void Awake()
        {
            base.Awake();
        }
    }
}
