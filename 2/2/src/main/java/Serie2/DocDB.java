package Serie2;

import java.util.concurrent.atomic.AtomicInteger;


public class DocDB { 

 private static class Doc { 
 private final   AtomicInteger version; 
 private final String text; 
 
 public Doc(int ver,  String txt) {
	 version=new AtomicInteger();
	 text = txt; 
	 } 
 } 

 private static class DocRef { 
	 public volatile  Doc ref; 
	 } 

 private AtomicInteger  _idx;  
 private final DocRef[] _store; 

 //OK porque o _store � final, logo no fim do construtor o Java garante com o final que este objecto est� construido
 public DocDB(int sz) { 
 _store = new DocRef[sz]; 
 for (int i = 0; i < sz; ++i) 
 _store[i] = new DocRef(); 
 _idx= new AtomicInteger(0);
 } 

 
 public int store(String text) {
	 int i;
 _store[ i=_idx.getAndIncrement()].ref = new Doc(0, text); 
 return i; 
 } 


 public String get(int id) { 
 return _store[id].ref.text; 
 } 

 public void update(int id, String newText) {

	 	
		 _store[id].ref = new Doc(_store[id].ref.version.incrementAndGet(), newText); // com o volatile no ref garanto que esta escrita � actualizada.

	}
	  
}