# Small and simple makefile for compiling IniMaster

.PHONY: all clean Arpar TestApplication

all: Arpar TestApplication

Arpar:
	$(MAKE) -C Arpar all

TestApplication:
	$(MAKE) -C TestApplication all

clean:
	$(MAKE) -C Arpar clean
	$(MAKE) -C TestApplication clean

