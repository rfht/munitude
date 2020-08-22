CC ?=		cc
CXX ?=		c++
CFLAGS ?=	-O2 -pipe
LDFLAGS =	-lmono-2.0 -L/usr/local/lib -Wl,-z,wxneeded
RM ?=		rm -f
TARGET ?=	munitude
SRCS =		munitude.c

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

Engine:
	$(MAKE) -C Engine

.PHONY: clean
clean:
	$(RM) $(TARGET) munitude.o Munitude.exe
	$(MAKE) -C Engine
