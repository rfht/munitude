RM ?=		rm -f

.PHONY: all
all: UnityEngine.dll UnityEngine.AnimationModule.dll UnityEngine.AudioModule.dll UnityEngine.CoreModule.dll UnityEngine.PhysicsModule.dll UnityEngine.Physics2DModule.dll UnityEngine.TextRenderingModule.dll UnityEngine.Timeline.dll UnityEngine.UI.dll

UnityEngine.dll: UnityEngine.cs
	csc -debug -target:library -out:UnityEngine.dll UnityEngine.cs

UnityEngine.AnimationModule.dll: UnityEngine.AnimationModule.cs
	csc -debug -target:library -out:UnityEngine.AnimationModule.dll UnityEngine.AnimationModule.cs

UnityEngine.AudioModule.dll: UnityEngine.AudioModule.cs
	csc -debug -target:library -out:UnityEngine.AudioModule.dll UnityEngine.AudioModule.cs

UnityEngine.CoreModule.dll: UnityEngine.CoreModule.cs
	csc -debug -target:library -out:UnityEngine.CoreModule.dll UnityEngine.CoreModule.cs

UnityEngine.PhysicsModule.dll: UnityEngine.PhysicsModule.cs
	csc -debug -target:library -out:UnityEngine.PhysicsModule.dll UnityEngine.PhysicsModule.cs

UnityEngine.Physics2DModule.dll: UnityEngine.Physics2DModule.cs
	csc -debug -target:library -out:UnityEngine.Physics2DModule.dll UnityEngine.Physics2DModule.cs

UnityEngine.TextRenderingModule.dll: UnityEngine.TextRenderingModule.cs
	csc -debug -target:library -out:UnityEngine.TextRenderingModule.dll UnityEngine.TextRenderingModule.cs

UnityEngine.Timeline.dll: UnityEngine.Timeline.cs
	csc -debug -target:library -out:UnityEngine.Timeline.dll UnityEngine.Timeline.cs

UnityEngine.UI.dll: UnityEngine.UI.cs
	csc -debug -target:library -out:UnityEngine.UI.dll UnityEngine.UI.cs

.PHONY: clean
clean:
	$(RM) *.dll *.pdb
