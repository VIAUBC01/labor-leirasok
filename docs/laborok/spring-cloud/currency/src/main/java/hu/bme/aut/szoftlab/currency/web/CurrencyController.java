package hu.bme.aut.szoftlab.currency.web;

import hu.bme.aut.szoftlab.currency.api.CurrencyApi;
import hu.bme.aut.szoftlab.currency.service.CurrencyService;
import hu.bme.aut.szoftlab.currency.service.RateDescriptor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api")
public class CurrencyController implements CurrencyApi {

    @Autowired
    private CurrencyService currencyService;
    
    @Override
    public double getRate(String from, String to) {
        return currencyService.getRate(new RateDescriptor(from, to));
    }
}
