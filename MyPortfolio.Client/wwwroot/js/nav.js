window.initNavObserver = () => {

    const sections = document.querySelectorAll("section");
    const navLinks = document.querySelectorAll(".nav-group a");

    if (!sections.length) return;

    const observer = new IntersectionObserver((entries) => { //for each section, check if it's intersecting and which one is most visible
        let mostVisible = null;
        let maxRatio = 0;

        entries.forEach(entry => {
            if (entry.isIntersecting && entry.intersectionRatio > maxRatio) {
                maxRatio = entry.intersectionRatio;
                mostVisible = entry;
            }
        });

        if (mostVisible) {

            navLinks.forEach(link => link.classList.remove("active"));

            const id = mostVisible.target.id;

            const activeLink = document.querySelector(`.nav-group a[href="#${id}"]`);

            if (activeLink) {
                activeLink.classList.add("active");
            }
        }

    }, {
        threshold: [0.6, 0.8]
    });

    sections.forEach(sec => observer.observe(sec));
};