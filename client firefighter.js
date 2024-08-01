mp.events.add('fireCreated', (x, y, z) => {
    mp.game.graphics.notify("~r~Пожар начался! Координаты: ~w~X: " + x + ", Y: " + y + ", Z: " + z);
    // Можно добавить маркер на карте
    const blip = mp.blips.new(436, new mp.Vector3(x, y, z), { name: "Пожар", color: 1, shortRange: false });
    setTimeout(() => blip.destroy(), 600000); // Удаляем маркер через 10 минут
});

mp.events.add('fireExtinguished', (x, y, z) => {
    mp.game.graphics.notify("~g~Пожар потушен! Координаты: ~w~X: " + x + ", Y: " + y + ", Z: " + z);
});
