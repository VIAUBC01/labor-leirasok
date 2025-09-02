package hu.bme.aut.szoftlab.flights.web;

import hu.bme.aut.szoftlab.flights.api.FlightsApi;
import hu.bme.aut.szoftlab.flights.dto.Airline;
import hu.bme.aut.szoftlab.flights.service.AirlineService;
import java.util.List;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api")
public class FlightsController implements FlightsApi {

    @Autowired
    private AirlineService airlineService;
    
    @Override
    public List<Airline> searchFlight(String from, String to) {
        return airlineService.search(from, to);
    }
}
