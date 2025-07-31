using System;
using System.Collections.Generic;
using AmplitudeNS.MiniJSON;
using UnityEngine;

public class Amplitude
{
	private static Dictionary<string, Amplitude> instances;

	private static readonly object instanceLock = new object();

	private static readonly string androidPluginName = "com.amplitude.unity.plugins.AmplitudePlugin";

	private AndroidJavaClass pluginClass;

	public bool logging;

	private string instanceName;

	public static Amplitude Instance
	{
		get
		{
			return getInstance();
		}
	}

	public Amplitude(string instanceName)
	{
		this.instanceName = instanceName;
		if (Application.platform == RuntimePlatform.Android)
		{
			Debug.Log("construct instance");
			pluginClass = new AndroidJavaClass(androidPluginName);
		}
	}

	public static Amplitude getInstance()
	{
		return getInstance(null);
	}

	public static Amplitude getInstance(string instanceName)
	{
		string text = instanceName;
		if (string.IsNullOrEmpty(text))
		{
			text = "$default_instance";
		}
		lock (instanceLock)
		{
			if (instances == null)
			{
				instances = new Dictionary<string, Amplitude>();
			}
			Amplitude value;
			if (instances.TryGetValue(text, out value))
			{
				return value;
			}
			value = new Amplitude(instanceName);
			instances.Add(text, value);
			return value;
		}
	}

	protected void Log(string message)
	{
		if (logging)
		{
			Debug.Log(message);
		}
	}

	public void init(string apiKey)
	{
		Log(string.Format("C# init {0}", apiKey));
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				using (AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getApplication", new object[0]))
				{
					pluginClass.CallStatic("init", instanceName, androidJavaObject, apiKey);
					pluginClass.CallStatic("enableForegroundTracking", instanceName, androidJavaObject2);
				}
			}
		}
	}

	public void init(string apiKey, string userId)
	{
		Log(string.Format("C# init {0} with userId {1}", apiKey, userId));
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				using (AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getApplication", new object[0]))
				{
					pluginClass.CallStatic("init", instanceName, androidJavaObject, apiKey, userId);
					pluginClass.CallStatic("enableForegroundTracking", instanceName, androidJavaObject2);
				}
			}
		}
	}

	public void setTrackingOptions(IDictionary<string, bool> trackingOptions)
	{
		if (trackingOptions != null)
		{
			string text = Json.Serialize(trackingOptions);
			Log(string.Format("C# setting tracking options {0}", text));
			if (Application.platform == RuntimePlatform.Android)
			{
				pluginClass.CallStatic("setTrackingOptions", instanceName, text);
			}
		}
	}

	public void logEvent(string evt)
	{
		Log(string.Format("C# sendEvent {0}", evt));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("logEvent", instanceName, evt);
		}
	}

	public void logEvent(string evt, IDictionary<string, object> properties)
	{
		string text = ((properties == null) ? Json.Serialize(new Dictionary<string, object>()) : Json.Serialize(properties));
		Log(string.Format("C# sendEvent {0} with properties {1}", evt, text));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("logEvent", instanceName, evt, text);
		}
	}

	public void logEvent(string evt, IDictionary<string, object> properties, bool outOfSession)
	{
		string text = ((properties == null) ? Json.Serialize(new Dictionary<string, object>()) : Json.Serialize(properties));
		Log(string.Format("C# sendEvent {0} with properties {1} and outOfSession {2}", evt, text, outOfSession));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("logEvent", instanceName, evt, text, outOfSession);
		}
	}

	public void setUserId(string userId)
	{
		Log(string.Format("C# setUserId {0}", userId));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserId", instanceName, userId);
		}
	}

	public void setUserProperties(IDictionary<string, object> properties)
	{
		string text = ((properties == null) ? Json.Serialize(new Dictionary<string, object>()) : Json.Serialize(properties));
		Log(string.Format("C# setUserProperties {0}", text));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserProperties", instanceName, text);
		}
	}

	public void setOptOut(bool enabled)
	{
		Log(string.Format("C# setOptOut {0}", enabled));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setOptOut", instanceName, enabled);
		}
	}

	[Obsolete("Please call setUserProperties instead", false)]
	public void setGlobalUserProperties(IDictionary<string, object> properties)
	{
		setUserProperties(properties);
	}

	public void logRevenue(double amount)
	{
		Log(string.Format("C# logRevenue {0}", amount));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("logRevenue", instanceName, amount);
		}
	}

	public void logRevenue(string productId, int quantity, double price)
	{
		Log(string.Format("C# logRevenue {0}, {1}, {2}", productId, quantity, price));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("logRevenue", instanceName, productId, quantity, price);
		}
	}

	public void logRevenue(string productId, int quantity, double price, string receipt, string receiptSignature)
	{
		Log(string.Format("C# logRevenue {0}, {1}, {2} (with receipt)", productId, quantity, price));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("logRevenue", instanceName, productId, quantity, price, receipt, receiptSignature);
		}
	}

	public void logRevenue(string productId, int quantity, double price, string receipt, string receiptSignature, string revenueType, IDictionary<string, object> eventProperties)
	{
		string text = ((eventProperties == null) ? Json.Serialize(new Dictionary<string, object>()) : Json.Serialize(eventProperties));
		Log(string.Format("C# logRevenue {0}, {1}, {2}, {3}, {4} (with receipt)", productId, quantity, price, revenueType, text));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("logRevenue", instanceName, productId, quantity, price, receipt, receiptSignature, revenueType, text);
		}
	}

	public string getDeviceId()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return pluginClass.CallStatic<string>("getDeviceId", new object[1] { instanceName });
		}
		return null;
	}

	public void regenerateDeviceId()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("regenerateDeviceId", instanceName);
		}
	}

	public void trackSessionEvents(bool enabled)
	{
		Log(string.Format("C# trackSessionEvents {0}", enabled));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("trackSessionEvents", instanceName, enabled);
		}
	}

	public long getSessionId()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return pluginClass.CallStatic<long>("getSessionId", new object[1] { instanceName });
		}
		return -1L;
	}

	public void clearUserProperties()
	{
		Log(string.Format("C# clearUserProperties"));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("clearUserProperties", instanceName);
		}
	}

	public void unsetUserProperty(string property)
	{
		Log(string.Format("C# unsetUserProperty {0}", property));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("unsetUserProperty", instanceName, property);
		}
	}

	public void setOnceUserProperty(string property, bool value)
	{
		Log(string.Format("C# setOnceUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, value);
		}
	}

	public void setOnceUserProperty(string property, double value)
	{
		Log(string.Format("C# setOnceUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, value);
		}
	}

	public void setOnceUserProperty(string property, float value)
	{
		Log(string.Format("C# setOnceUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, value);
		}
	}

	public void setOnceUserProperty(string property, int value)
	{
		Log(string.Format("C# setOnceUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, value);
		}
	}

	public void setOnceUserProperty(string property, long value)
	{
		Log(string.Format("C# setOnceUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, value);
		}
	}

	public void setOnceUserProperty(string property, string value)
	{
		Log(string.Format("C# setOnceUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, value);
		}
	}

	public void setOnceUserProperty(string property, IDictionary<string, object> values)
	{
		if (values != null)
		{
			string text = Json.Serialize(values);
			Log(string.Format("C# setOnceUserProperty {0}, {1}", property, text));
			if (Application.platform == RuntimePlatform.Android)
			{
				pluginClass.CallStatic("setOnceUserPropertyDict", instanceName, property, text);
			}
		}
	}

	public void setOnceUserProperty<T>(string property, IList<T> values)
	{
		if (values != null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("list", values);
			Dictionary<string, object> obj = dictionary;
			string text = Json.Serialize(obj);
			Log(string.Format("C# setOnceUserProperty {0}, {1}", property, text));
			if (Application.platform == RuntimePlatform.Android)
			{
				pluginClass.CallStatic("setOnceUserPropertyList", instanceName, property, text);
			}
		}
	}

	public void setOnceUserProperty(string property, bool[] array)
	{
		Log(string.Format("C# setOnceUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, array);
		}
	}

	public void setOnceUserProperty(string property, double[] array)
	{
		Log(string.Format("C# setOnceUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, array);
		}
	}

	public void setOnceUserProperty(string property, float[] array)
	{
		Log(string.Format("C# setOnceUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, array);
		}
	}

	public void setOnceUserProperty(string property, int[] array)
	{
		Log(string.Format("C# setOnceUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, array);
		}
	}

	public void setOnceUserProperty(string property, long[] array)
	{
		Log(string.Format("C# setOnceUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, array);
		}
	}

	public void setOnceUserProperty(string property, string[] array)
	{
		Log(string.Format("C# setOnceUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, array);
		}
	}

	public void setUserProperty(string property, bool value)
	{
		Log(string.Format("C# setUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserProperty", instanceName, property, value);
		}
	}

	public void setUserProperty(string property, double value)
	{
		Log(string.Format("C# setUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserProperty", instanceName, property, value);
		}
	}

	public void setUserProperty(string property, float value)
	{
		Log(string.Format("C# setUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserProperty", instanceName, property, value);
		}
	}

	public void setUserProperty(string property, int value)
	{
		Log(string.Format("C# setUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserProperty", instanceName, property, value);
		}
	}

	public void setUserProperty(string property, long value)
	{
		Log(string.Format("C# setUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserProperty", instanceName, property, value);
		}
	}

	public void setUserProperty(string property, string value)
	{
		Log(string.Format("C# setUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserProperty", instanceName, property, value);
		}
	}

	public void setUserProperty(string property, IDictionary<string, object> values)
	{
		if (values != null)
		{
			string text = Json.Serialize(values);
			Log(string.Format("C# setUserProperty {0}, {1}", property, text));
			if (Application.platform == RuntimePlatform.Android)
			{
				pluginClass.CallStatic("setUserProperty", instanceName, property, text);
			}
		}
	}

	public void setUserProperty<T>(string property, IList<T> values)
	{
		if (values != null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("list", values);
			Dictionary<string, object> obj = dictionary;
			string text = Json.Serialize(obj);
			Log(string.Format("C# setUserProperty {0}, {1}", property, text));
			if (Application.platform == RuntimePlatform.Android)
			{
				pluginClass.CallStatic("setUserPropertyList", instanceName, property, text);
			}
		}
	}

	public void setUserProperty(string property, bool[] array)
	{
		Log(string.Format("C# setUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserProperty", instanceName, property, array);
		}
	}

	public void setUserProperty(string property, double[] array)
	{
		Log(string.Format("C# setUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserProperty", instanceName, property, array);
		}
	}

	public void setUserProperty(string property, float[] array)
	{
		Log(string.Format("C# setUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserProperty", instanceName, property, array);
		}
	}

	public void setUserProperty(string property, int[] array)
	{
		Log(string.Format("C# setUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserProperty", instanceName, property, array);
		}
	}

	public void setUserProperty(string property, long[] array)
	{
		Log(string.Format("C# setUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserProperty", instanceName, property, array);
		}
	}

	public void setUserProperty(string property, string[] array)
	{
		Log(string.Format("C# setUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("setUserProperty", instanceName, property, array);
		}
	}

	public void addUserProperty(string property, double value)
	{
		Log(string.Format("C# addUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("addUserProperty", instanceName, property, value);
		}
	}

	public void addUserProperty(string property, float value)
	{
		Log(string.Format("C# addUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("addUserProperty", instanceName, property, value);
		}
	}

	public void addUserProperty(string property, int value)
	{
		Log(string.Format("C# addUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("addUserProperty", instanceName, property, value);
		}
	}

	public void addUserProperty(string property, long value)
	{
		Log(string.Format("C# addUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("addUserProperty", instanceName, property, value);
		}
	}

	public void addUserProperty(string property, string value)
	{
		Log(string.Format("C# addUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("addUserProperty", instanceName, property, value);
		}
	}

	public void addUserProperty(string property, IDictionary<string, object> values)
	{
		if (values != null)
		{
			string text = Json.Serialize(values);
			Log(string.Format("C# addUserProperty {0}, {1}", property, text));
			if (Application.platform == RuntimePlatform.Android)
			{
				pluginClass.CallStatic("addUserPropertyDict", instanceName, property, text);
			}
		}
	}

	public void appendUserProperty(string property, bool value)
	{
		Log(string.Format("C# appendUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("appendUserProperty", instanceName, property, value);
		}
	}

	public void appendUserProperty(string property, double value)
	{
		Log(string.Format("C# appendUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("appendUserProperty", instanceName, property, value);
		}
	}

	public void appendUserProperty(string property, float value)
	{
		Log(string.Format("C# appendUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("appendUserProperty", instanceName, property, value);
		}
	}

	public void appendUserProperty(string property, int value)
	{
		Log(string.Format("C# appendUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("appendUserProperty", instanceName, property, value);
		}
	}

	public void appendUserProperty(string property, long value)
	{
		Log(string.Format("C# appendUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("appendUserProperty", instanceName, property, value);
		}
	}

	public void appendUserProperty(string property, string value)
	{
		Log(string.Format("C# appendUserProperty {0}, {1}", property, value));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("appendUserProperty", instanceName, property, value);
		}
	}

	public void appendUserProperty(string property, IDictionary<string, object> values)
	{
		if (values != null)
		{
			string text = Json.Serialize(values);
			Log(string.Format("C# appendUserProperty {0}, {1}", property, text));
			if (Application.platform == RuntimePlatform.Android)
			{
				pluginClass.CallStatic("appendUserPropertyDict", instanceName, property, text);
			}
		}
	}

	public void appendUserProperty<T>(string property, IList<T> values)
	{
		if (values != null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("list", values);
			Dictionary<string, object> obj = dictionary;
			string text = Json.Serialize(obj);
			Log(string.Format("C# appendUserProperty {0}, {1}", property, text));
			if (Application.platform == RuntimePlatform.Android)
			{
				pluginClass.CallStatic("appendUserPropertyList", instanceName, property, text);
			}
		}
	}

	public void appendUserProperty(string property, bool[] array)
	{
		Log(string.Format("C# appendUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("appendUserProperty", instanceName, property, array);
		}
	}

	public void appendUserProperty(string property, double[] array)
	{
		Log(string.Format("C# appendUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("appendUserProperty", instanceName, property, array);
		}
	}

	public void appendUserProperty(string property, float[] array)
	{
		Log(string.Format("C# appendUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("appendUserProperty", instanceName, property, array);
		}
	}

	public void appendUserProperty(string property, int[] array)
	{
		Log(string.Format("C# appendUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("appendUserProperty", instanceName, property, array);
		}
	}

	public void appendUserProperty(string property, long[] array)
	{
		Log(string.Format("C# appendUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("appendUserProperty", instanceName, property, array);
		}
	}

	public void appendUserProperty(string property, string[] array)
	{
		Log(string.Format("C# appendUserProperty {0}, {1}", property, array));
		if (Application.platform == RuntimePlatform.Android)
		{
			pluginClass.CallStatic("appendUserProperty", instanceName, property, array);
		}
	}

	public void startSession()
	{
	}

	public void endSession()
	{
	}
}
