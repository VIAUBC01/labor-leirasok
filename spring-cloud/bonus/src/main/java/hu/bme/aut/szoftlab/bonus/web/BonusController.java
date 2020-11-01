package hu.bme.aut.szoftlab.bonus.web;

import hu.bme.aut.szoftlab.bonus.api.BonusApi;
import hu.bme.aut.szoftlab.bonus.model.Bonus;
import hu.bme.aut.szoftlab.bonus.repository.BonusRepository;
import hu.bme.aut.szoftlab.bonus.service.BonusService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.server.ResponseStatusException;

@RestController
@RequestMapping("/api")
public class BonusController implements BonusApi {

    @Autowired
    BonusRepository bonusRepository;
    
    @Autowired
    BonusService bonusService;
    
    @Override
    public double getPoints(String user) {
        return bonusRepository.findById(user)
                .orElseGet(Bonus::new)
                .getPoints();
    }
    
    @Override
    public double addPoints(String user, double pointsToAdd) {
        try {
            return bonusService.addPoints(user, pointsToAdd);
        } catch(IllegalArgumentException e) {
           throw new ResponseStatusException(HttpStatus.BAD_REQUEST);
        }
    }
}
