package hu.bme.aut.szoftlab.bonus.service;

import hu.bme.aut.szoftlab.bonus.model.Bonus;
import hu.bme.aut.szoftlab.bonus.repository.BonusRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class BonusService {

    @Autowired
    BonusRepository bonusRepository;

    @Transactional
    public double addPoints(String user, double pointsToAdd) {
        Bonus bonus = bonusRepository.findById(user)
                .orElseGet(() -> bonusRepository.save(new Bonus(user, 0.0)));

        if (-1 * pointsToAdd > bonus.getPoints())
            throw new IllegalArgumentException();
        bonus.setPoints(bonus.getPoints() + pointsToAdd);
        return bonus.getPoints();
    }
}
