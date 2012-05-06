# Small and simple makefile for compiling IniMaster
# Usage: make -f makefile.mono

TARGET=bin
LIBRARY=../Arpar/bin/Arpar.dll

# no flags for now
CSC_FLAGS=

TESTAPPEXE=$(TARGET)/TestApplication.exe

SOURCES= Program.cs	\
	ExampleProgram1.cs \
	ExampleProgram2.cs \
	ExampleSettings1.cs \
	ExampleSettings2.cs \
	Properties/AssemblyInfo.cs

CSC = gmcs

all: clean $(TESTAPPEXE)

$(TESTAPPEXE):
	cp $(LIBRARY) "$(TARGET)/"
	$(CSC) $(CSC_FLAGS) -r:"$(TARGET)/Arpar.dll" -r:System.dll -r:System.Data.dll -r:System.Xml.dll /out:$(TESTAPPEXE) $(SOURCES)

clean:
	rm -rf "$(TESTAPPEXE)" "$(TARGET)/Arpar.dll"

