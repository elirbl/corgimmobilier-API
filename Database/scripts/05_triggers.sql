-- ============================================================
-- 05_triggers.sql
-- Maintien automatique de "UpdatedAt" sur la table "Properties"
-- ============================================================

CREATE FUNCTION update_updated_at()
RETURNS TRIGGER AS $$
BEGIN
    NEW."UpdatedAt" := now();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_properties_updated_at
    BEFORE UPDATE ON "Properties"
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at();
