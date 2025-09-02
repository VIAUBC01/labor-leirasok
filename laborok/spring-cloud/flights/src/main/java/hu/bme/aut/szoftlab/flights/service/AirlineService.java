package hu.bme.aut.szoftlab.flights.service;

import hu.bme.aut.szoftlab.flights.dto.Airline;
import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;
import org.springframework.stereotype.Service;

@Service
public class AirlineService {

    private List<Airline> airlines = Arrays.asList(
            new Airline(1, "Budapest", "Lhasa", "HUF", 150000),
            new Airline(2, "Lhasa", "Budapest", "HUF", 130000), 
            new Airline(1, "Budapest", "Praga", "EUR", 150),
            new Airline(1, "Praga", "Budapest", "EUR", 120)

    );

    public List<Airline> search(String from, String to) {
        return airlines.stream()
                .filter(a -> a.getFrom().equals(from) 
                        && a.getTo().equals(to))
                .collect(Collectors.toList());
    }
}
