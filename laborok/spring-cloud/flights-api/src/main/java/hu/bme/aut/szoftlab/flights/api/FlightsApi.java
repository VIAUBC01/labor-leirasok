package hu.bme.aut.szoftlab.flights.api;

import hu.bme.aut.szoftlab.flights.dto.Airline;
import java.util.List;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;

public interface FlightsApi {

    @GetMapping("/flight/{from}/{to}")
    List<Airline> searchFlight(@PathVariable("from") String from, @PathVariable("to") String to);

}