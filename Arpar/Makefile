# Small and simple makefile for compiling IniMaster
# Usage: make -f makefile.mono

TARGET=bin

# no flags for now
CSC_FLAGS=

INIMASTERCOREDLL=$(TARGET)/Arpar.dll

SOURCES= Types.cs	\
	ArgumentAliasAttribute.cs \
	ArgumentAttribute.cs \
	Argument.cs \
	ArgumentParser.cs \
	BoundedArgumentAttribute.cs \
	ChoicesArgumentAttribute.cs \
	CommonArgumentAttribute.cs \
	DuplicateArgumentException.cs \
	UnknownArgumentException.cs \
	Properties/AssemblyInfo.cs

CSC = gmcs

all: clean $(INIMASTERCOREDLL)

$(INIMASTERCOREDLL):
	$(CSC) $(CSC_FLAGS) -r:System.dll -r:System.Data.dll -r:System.Xml.dll -target:library /out:$(INIMASTERCOREDLL) $(SOURCES)

clean:
	rm -rf "$(INIMASTERCOREDLL)"

