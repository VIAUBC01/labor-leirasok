package hu.bme.aut.szoftlab.bonus.api;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;

public interface BonusApi {

    @GetMapping("/bonus/{user}")
    double getPoints(@PathVariable("user") String user);

    @PutMapping("/bonus/{user}")
    double addPoints(@PathVariable("user") String user, @RequestBody double pointsToAdd);

}