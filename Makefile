CC ?=		cc
CXX ?=		c++
CFLAGS ?=	-O2 -pipe
LDFLAGS =	-lmono-2.0 -L/usr/local/lib -Wl,-z,wxneeded
RM ?=		rm -f
TARGET ?=	munitude
SRCS =		munitude.c internal_calls.c

.PHONY: all
all: $(TARGET) Munitude.exe Engine

Munitude.exe: Munitude.cs 0Harmony.dll
	csc -debug -r:0Harmony.dll Munitude.cs

$(TARGET): munitude.o internal_calls.o
	$(CC) $(LDFLAGS) -o $(TARGET) munitude.o internal_calls.o

munitude_cpp:
	$(CXX) $(CFLAGS) -I/usr/local/include/mono-2.0 -c -fPIC -Wall -Werror munitude_cpp.cpp
	$(CXX) $(LDFLAGS) -o munitude_cpp munitude_cpp.o

munitude.o:
	$(CC) $(CFLAGS) -I/usr/local/include/mono-2.0 -c -fPIC -Wall -Werror munitude.c

internal_calls.o:
	$(CC) $(CFLAGS) -I/usr/local/include/mono-2.0 -c -fPIC -Wall -Werror internal_calls.c

Engine:
	$(MAKE) -C Engine

.PHONY: clean
clean:
	$(RM) $(TARGET) munitude_cpp munitude_cpp.o munitude.o Munitude.exe
	$(MAKE) -C Engine clean
