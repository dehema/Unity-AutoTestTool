public enum ViewLayer
{ 
	//普通UI
	NormalUI = 0,
	//游戏菜单UI
	MenuUI = 5,
	//弹窗UI
	PopUI = 10,
	//提示UI
	TipsUI = 20,
	//引导UI
	GuideUI = 25,
	//阻挡点击UI
	PreventUI = 27,
	//光标特效
	CursorUI = 28,
}

public class ViewName
{
	public const string DebugView = "DebugView";
	public const string BlockView = "BlockView";
	public const string TopView = "TopView";
	public const string LoadingView = "LoadingView";
	public const string ExampleView = "ExampleView";
	public const string LoadSceneView = "LoadSceneView";
	public const string MainView = "MainView";
	public const string ActionView = "ActionView";
	public const string ActionMouseClickView = "ActionMouseClickView";
	public const string TipsView = "TipsView";
	public const string PopConfirmView = "PopConfirmView";
	public const string ActionScreenCaptureIdentifyView = "ActionScreenCaptureIdentifyView";
	public const string ScriptBriefView = "ScriptBriefView";
	public const string PopInfoView = "PopInfoView";
}