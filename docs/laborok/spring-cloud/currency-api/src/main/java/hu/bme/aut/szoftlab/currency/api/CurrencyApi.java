package hu.bme.aut.szoftlab.currency.api;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;

public interface CurrencyApi {

  @GetMapping("/rate/{from}/{to}")
  double getRate(@PathVariable("from") String from, @PathVariable("to") String to);

}