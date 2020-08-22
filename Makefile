CC ?=		cc
CXX ?=		c++
CFLAGS ?=	-O0 -pipe -g
LDFLAGS =	-lmono-2.0 -L/usr/local/lib -Wl,-z,wxneeded
RM ?=		rm -f
TARGET ?=	munitude
SRCS =		munitude.c
MAJOR ?=	0
MINOR ?=	0

.PHONY: all
all: $(TARGET) Munitude.exe UnityEngine

Munitude.exe: Munitude.cs
	csc -debug Munitude.cs

$(TARGET): munitude.o
	$(CC) $(LDFLAGS) -o $(TARGET) munitude.o

munitude_cpp:
	$(CXX) $(CFLAGS) -I/usr/local/include/mono-2.0 -c -fPIC -Wall -Werror munitude.cpp
	$(CXX) $(LDFLAGS) -o munitude_cpp munitude.o

munitude.o:
	$(CC) $(CFLAGS) -I/usr/local/include/mono-2.0 -c -fPIC -Wall -Werror $(SRCS)

UnityEngine: UnityEngine.AudioModule.dll UnityEngine.CoreModule.dll

UnityEngine.AudioModule.dll: UnityEngine.AudioModule.cs
	csc -debug -target:library -out:UnityEngine.AudioModule.dll UnityEngine.AudioModule.cs

UnityEngine.CoreModule.dll: UnityEngine.CoreModule.cs
	csc -debug -target:library -out:UnityEngine.CoreModule.dll UnityEngine.CoreModule.cs

.PHONY: clean
clean:
	$(RM) $(TARGET) munitude.o Munitude.exe UnityEngine.CoreModule.dll
