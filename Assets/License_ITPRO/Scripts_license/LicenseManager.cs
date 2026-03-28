using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

namespace ITPRO.License
{
	public class LicenseManager : MonoBehaviour
	{
		// public static string LicUriMain => "https://nuclearbox.org:13044/Home/Activate";
		// public static string LicUriMain => "http://10.1.5.250:8082/Home/Activate";
		public static string LicUriMain => "http://77.37.140.116:12053/Home/Activate";
		public static string LicUriDebug => "http://77.37.140.116:12053/Home/Activate";
		public static string LicUriFinal { get; private set; }

		public static string LicFilePath => Directory.GetCurrentDirectory() + "\\lic.txt";
		public static string LicDebugPath => Directory.GetCurrentDirectory() + "\\debug.txt";

		public static string Key;

		public string ApplicationName;
		public int LoadingSceneId;
		public TMP_InputField Key_InputField;
		public TMP_Text Error_Label;
		public ErrorMessages messages;
		public LicenseLoader loader;
		bool inited;

		IEnumerator curCor;

		private void Awake()
		{
			if (File.Exists(LicFilePath))
			{
				Key_InputField.text = File.ReadAllText(LicFilePath);
			}
		}

		public void Init()
		{
			if (inited)
				return;

			if (File.Exists(LicDebugPath))
			{
				LicUriFinal = LicUriDebug;
			}
			else
			{
				LicUriFinal = LicUriMain;
			}

			inited = true;
		}

		public void CheckLicense()
		{
			Init();

			// Debug.Log("CheckLicense [" + Key + "]");
			if (Key_InputField.text.Length > 0)
			{
				Key = Key_InputField.text;
				StartCoroutine(SendLicData());
			}
			else
			{
				messages.ActivateError(Error.Input);
				//StartCoroutine(curCor = ShowNotification("Введите ключ!"));
			}
		}

		public void Exit()
		{
			Application.Quit();
		}

		IEnumerator SendLicData()
		{
			Debug.Log("SendLicData [" + Key + "] [" + SystemInfo.deviceUniqueIdentifier + "] [" + ApplicationName + "] to " + LicUriFinal);
			LicRequest licRequest = new LicRequest() { key = Key, deviceId = SystemInfo.deviceUniqueIdentifier, appName = ApplicationName };

			string jsonToSend = JsonConvert.SerializeObject(licRequest);
			Debug.Log("jsonToSend " + jsonToSend);

			UnityWebRequest req = new UnityWebRequest();
			req.url = LicUriFinal;
			req.method = "POST";
			req.downloadHandler = new DownloadHandlerBuffer();
			req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonToSend));

			req.SetRequestHeader("Content-Type", "application/json");
			req.SetRequestHeader("Accept", "application/json");

			yield return req.SendWebRequest();

			while (!req.isDone)
			{
				yield return null;
			}

			if (Application.internetReachability != NetworkReachability.NotReachable)
			{
				try
				{
					// Debug.Log("[" + req.downloadHandler.text + "]");
					Debug.Log($"[{req.downloadHandler.text}]");

					LicResult licResult = JsonConvert.DeserializeObject<LicResult>(req.downloadHandler.text);

					if (licResult.status == null)
					{
						if (File.Exists(LicFilePath)) File.Delete(LicFilePath);
						messages.ActivateError(Error.Server);
						//StartCoroutine(curCor = ShowNotification("Ответ от сервера пустой!"));
					}
					else
					{
						switch (licResult.status)
						{
							case "ok":
								{
									//if (curCor != null) StopCoroutine(curCor);
									// StartCoroutine(curCor = ShowNotification("License is OK"));
									File.WriteAllText(LicFilePath, Key);
									//SceneManager.LoadScene(LoadingSceneId);
									loader.StartLoad();
									Debug.Log("License is OK");
									break;
								}
							case "key not found":
								{
									if (File.Exists(LicFilePath)) File.Delete(LicFilePath);
									//if (curCor != null) StopCoroutine(curCor);
									messages.ActivateError(Error.Key);
									//StartCoroutine(curCor = ShowNotification("Ключ введен неверно!"));
									Debug.Log("License ERROR: key not found");
									break;
								}
							case "max amount of activations":
								{
									if (File.Exists(LicFilePath)) File.Delete(LicFilePath);
									if (curCor != null) StopCoroutine(curCor);
									messages.ActivateError(Error.Max);
									//StartCoroutine(curCor = ShowNotification("Ключ использован максимальное количество раз!"));
									Debug.Log("License ERROR: max amount of activations");
									break;
								}
							case "key expired":
								{
									if (File.Exists(LicFilePath)) File.Delete(LicFilePath);
									//if (curCor != null) StopCoroutine(curCor);
									messages.ActivateError(Error.Time);
									//StartCoroutine(curCor = ShowNotification("Время действия ключа истекло!"));
									Debug.Log("License ERROR: key expired");
									break;
								}
							case "key not for this app":
								{
									if (File.Exists(LicFilePath)) File.Delete(LicFilePath);
									//if (curCor != null) StopCoroutine(curCor);
									messages.ActivateError(Error.App);
									//StartCoroutine(curCor = ShowNotification("Ключ не действителен для данного приложения!"));
									Debug.Log("License ERROR: key not for this application");
									break;
								}
							case "error":
								{
									if (File.Exists(LicFilePath)) File.Delete(LicFilePath);
									//if (curCor != null) StopCoroutine(curCor);
									messages.ActivateSpecialError(Error.License, "License ERROR: " + licResult.error);
									//StartCoroutine(curCor = ShowNotification("License ERROR: " + licResult.error));
									Debug.Log("License ERROR: " + licResult.error);
									break;
								}
							default:
								{
									if (File.Exists(LicFilePath)) File.Delete(LicFilePath);
									//if (curCor != null) StopCoroutine(curCor);
									messages.ActivateError(Error.Unknow);
									//StartCoroutine(curCor = ShowNotification("Неопознанная ошибка!"));
									Debug.Log("License ERROR: Unknown error");
									break;
								}
						}
					}
				}
				catch
				{
					//if (curCor != null) StopCoroutine(curCor);
					messages.ActivateError(Error.Json);
					//StartCoroutine(curCor = ShowNotification("Ответ от сервера неожиданный (json)!"));
					// Debug.LogWarning(req.result.ToString());
					Debug.Log("License ERROR: Ответ от сервера неожиданный (json)!");
				}
			}
			else
			{
				//if (curCor != null) StopCoroutine(curCor);
				messages.ActivateSpecialError(Error.Connect, "Ошибка соединения");
				//StartCoroutine(curCor = ShowNotification("Ошибка соединения: " + req.result.ToString()));
				Debug.Log("License ERROR: Ошибка соединения");
			}
		}

		//private IEnumerator ShowNotification(string notText)
		//      {
		//	Error_Label.text = notText;
		//	yield return new WaitForSeconds(4);
		//	Error_Label.text = "";
		//}

		class LicResult
		{
			public string status;
			public string error;
		}

		class LicRequest
		{
			public string key;
			public string deviceId;
			public string appName;
		}

		//class BypassCertificate : CertificateHandler
		//{
		//	protected override bool ValidateCertificate(byte[] certificateData)
		//	{
		//		//Simply return true no matter what
		//		return true;
		//	}
		//}
	}
}
