package md53cb23a7ea6337873dd28cc9b3d76d964;


public class CreaBus
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("AppAndroid.Acti.CreaBus, AppAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CreaBus.class, __md_methods);
	}


	public CreaBus () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CreaBus.class)
			mono.android.TypeManager.Activate ("AppAndroid.Acti.CreaBus, AppAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
